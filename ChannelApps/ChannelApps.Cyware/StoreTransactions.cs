using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using KTI.Moo.Extensions.Cyware.Domain;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Magento.Model;
using Moo.FO.Model;
using Moo.FO.Model.DTO;
using Moo.FO.Model.DTO.Batch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace ChannelApps.Cyware
{
    public class StoreTransactions
    {
        private readonly QueueClient _queueClient;
        private readonly IBlobService _blobService;
        private readonly string _storetransactioncontainer;
        public StoreTransactions(string connectionString, IBlobService blobService, string storetransactioncontainer)
        {
            _queueClient = new QueueClient(connectionString, "fo-storetransactions-queue", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            _queueClient.CreateIfNotExists();
            _blobService = blobService;
            _storetransactioncontainer = storetransactioncontainer;
        }

        public async Task<bool> StoreTransactionsProcess(List<FOSalesTransactionHeader> headers, List<FOSalesTransactionDetail> lines, List<FOSalesTransactionDiscount> discounts, List<FOSalesTenderDetail> tenders, D365FOConfig config)
        {
            List<FOSalesTransactionHeader> sortedHeaders = headers.OrderBy(item =>
            {
                int parsedValue;
                return int.TryParse(item.TransactionNumber, out parsedValue) ? parsedValue : int.MaxValue;
            }).ToList();

            List<FOSalesTransactionHeader> excessTransactions = new();

            int timestampCounter = 1;

            while (sortedHeaders.Any() || excessTransactions.Any())
            {
                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() + timestampCounter.ToString();
                timestampCounter++;

                List<FOSalesTransactionHeader> headersToProcess = excessTransactions.Any() ? excessTransactions.Take(20).ToList() : sortedHeaders.Take(20).ToList();
                if (excessTransactions.Any())
                {
                    excessTransactions.RemoveRange(0, Math.Min(20, excessTransactions.Count));
                }

                excessTransactions.AddRange(await ProcessTransactions(headersToProcess, lines, discounts, tenders, timestamp, config));

                // Remove the processed headers from the main list
                sortedHeaders.RemoveRange(0, Math.Min(20, headersToProcess.Count));
            }

            return true;
        }

        private async Task<List<FOSalesTransactionHeader>> ProcessTransactions(List<FOSalesTransactionHeader> headersToProcess, List<FOSalesTransactionDetail> lines, List<FOSalesTransactionDiscount> discounts, List<FOSalesTenderDetail> tenders, string timestamp, D365FOConfig config)
        {
            int totalLines = 0;
            int totalDiscounts = 0;
            int totalTenders = 0;
            List<FOSalesTransactionHeader> transactionsToProcess = new();
            List<FOSalesTransactionHeader> excessTransactions = new();

            foreach (var header in headersToProcess)
            {
                var headerLines = lines.Where(line => line.TransactionNumber == header.TransactionNumber && line.Terminal == header.Terminal).ToList();
                var headerDiscounts = discounts.Where(discount => discount.TransactionNumber == header.TransactionNumber && discount.Terminal == header.Terminal).ToList();
                var headerTenders = tenders.Where(tender => tender.TransactionNumber == header.TransactionNumber && tender.Terminal == header.Terminal).ToList();

                int headerLineCount = headerLines.Count;
                int headerDiscountCount = headerDiscounts.Count;
                int headerTenderCount = headerTenders.Count;

                if (totalLines + headerLineCount <= 200 && totalDiscounts + headerDiscountCount <= 200 && totalTenders + headerTenderCount <= 200)
                {
                    transactionsToProcess.Add(header);
                    totalLines += headerLineCount;
                    totalDiscounts += headerDiscountCount;
                    totalTenders += headerTenderCount;
                }
                else
                {
                    // Add the headers to excess transaction for processing later if the accumulated count exceeds 200
                    excessTransactions.Add(header);
                }
            }

            string transformedHeaders = transactionsToProcess
                .Select((transaction, index) => FormatRequest(index + 1, timestamp, "RetailTransactions", new FO_StoreTransactionHeaderDTO(transaction, config).ToString()))
                .Aggregate("", (current, next) => current + next);

            int lineContentId = 0;
            string transformedLines = lines
                .Select((line, index) =>
                {
                    string storeNumber = "";
                    string register = line.Terminal;
                    if (line.Terminal.Contains('-'))
                    {
                        string[] storeAndRegister = register.Split('-');
                        storeNumber = storeAndRegister[0];
                        register = storeAndRegister[1];
                    }
                    if (String.IsNullOrEmpty(storeNumber))
                    {
                        throw new Exception($"[Register: {register}, Transaction Number: {line.TransactionNumber}, StoreNumber is null or empty");
                    }
                    var header = GetHeader(storeNumber, register, line.TransactionNumber, transactionsToProcess);
                    if (header != null)
                        lineContentId++;

                    return header != null ? FormatRequest(lineContentId, timestamp, "RetailTransactionSalesLinesV2", new FO_StoreTransactionLinesDTO(line, header, config).ToString()) : null;
                })
                .Where(dto => dto != null) // Filter out null elements (where transaction numbers didn't match any headers)
                .Aggregate("", (current, next) => current + next);

            int discountContentId = 0;
            string transformedDiscounts = discounts.Any()
                ? discounts
                    .Select((discount, index) =>
                    {
                        string storeNumber = "";
                        string register = discount.Terminal;
                        if (discount.Terminal.Contains('-'))
                        {
                            string[] storeAndRegister = register.Split('-');
                            storeNumber = storeAndRegister[0];
                            register = storeAndRegister[1];
                        }
                        if (String.IsNullOrEmpty(storeNumber))
                        {
                            throw new Exception($"[Register: {register}, Transaction Number: {discount.TransactionNumber}, StoreNumber is null or empty");
                        }
                        var header = GetHeader(storeNumber, register, discount.TransactionNumber, transactionsToProcess);
                        if (header != null)
                            discountContentId++;

                        return header != null ? FormatRequest(discountContentId, timestamp, "RetailTransactionDiscountLines", new FO_StoreDiscountsDTO(discount, config).ToString()) : null;
                    })
                    .Where(dto => dto != null) // Filter out null elements (where transaction numbers didn't match any headers)
                    .Aggregate("", (current, next) => current + next)
                : "";

            int tendersContentId = 0;
            string transformedtenders = tenders
                .Select((tender, index) =>
                {
                    var header = GetHeader(tender.Store, tender.Terminal, tender.TransactionNumber, transactionsToProcess);
                    if (header != null)
                        tendersContentId++;

                    return header != null ? FormatRequest(tendersContentId, timestamp, "RetailTransactionPaymentLinesV2", new FO_StoreTendersDTO(tender, config).ToString()) : null;
                })
                .Where(dto => dto != null) // Filter out null elements (where transaction numbers didn't match any headers)
                .Aggregate("", (current, next) => current + next);

            KTI.Moo.Extensions.Cyware.Model.StoreTransactions blobFiles = await CreateBlob(timestamp, transformedHeaders, transformedLines, transformedDiscounts, transformedtenders);

            addStoreTransactionsToQueue(blobFiles);

            return excessTransactions;
        }


        private async Task<KTI.Moo.Extensions.Cyware.Model.StoreTransactions> CreateBlob(string timestamp, string headers, string lines, string discounts, string tenders)
        {
            string headersBlob = $"FO/Headers/batch_{timestamp}.txt";
            string linesBlob = $"FO/Lines/batch_{timestamp}.txt";
            string discountsBlob = $"FO/Discounts/batch_{timestamp}.txt";
            string paymentsBlob = $"FO/Tenders/batch_{timestamp}.txt";

            await _blobService.CreateFile(_storetransactioncontainer, headersBlob, headers);
            await _blobService.CreateFile(_storetransactioncontainer, linesBlob, lines);
            if (!string.IsNullOrEmpty(discounts))
            {
                await _blobService.CreateFile(_storetransactioncontainer, discountsBlob, discounts);
            }
            else
            {
                discountsBlob = "";
            }
            await _blobService.CreateFile(_storetransactioncontainer, paymentsBlob, tenders);

            KTI.Moo.Extensions.Cyware.Model.StoreTransactions storeTransactionsBlob = new()
            {
                Headers = headersBlob,
                Lines = linesBlob,
                Discounts = discountsBlob,
                Payments = paymentsBlob
            };

            return storeTransactionsBlob;
        }

        private string FormatRequest(int counter, string timestamp, string apiURL, string body)
        {
            string batchRequestTemplate = @"--batch_{timestamp}
Content-Type: multipart/mixed;boundary=changeset_{timestamp}{counter}

--changeset_{timestamp}{counter}
Content-Type: application/http
Content-Transfer-Encoding:binary

POST {api_url}?cross-company=true HTTP/1.1
Content-ID: {counter}
Accept: application/json;q=0.9, */*;q=0.1
OData-Version: 4.0
Content-Type: application/json
OData-MaxVersion: 4.0

{body}
--changeset_{timestamp}{counter}--

";

            string batchRequestFormat = batchRequestTemplate
                .Replace("{timestamp}", timestamp)
                .Replace("{counter}", counter.ToString())
                .Replace("{api_url}", apiURL)
                .Replace("{body}", body);

            return batchRequestFormat;
        }

        private FOSalesTransactionHeader GetHeader(string storeNumber, string registerNumber, string transactionNumber, List<FOSalesTransactionHeader> headers)
        {
            return headers.FirstOrDefault(header =>
                    header.ChannelReferenceId == storeNumber &&
                    header.Terminal == registerNumber &&
                    header.TransactionNumber == transactionNumber);
        }

        private bool addStoreTransactionsToQueue(KTI.Moo.Extensions.Cyware.Model.StoreTransactions storeTransactions)
        {
            // Convert StoreTransactions object to a JSON string
            string jsonBlobFiles = JsonConvert.SerializeObject(storeTransactions);

            // Send the JSON string as a queue message
            _queueClient.SendMessage(jsonBlobFiles);
            return true;
        }
    }
}
