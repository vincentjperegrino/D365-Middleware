using KTI.Moo.Base.Domain.Queue;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Moo.FO.App.Queue.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Moo.FO.App.Queue.Notification
{
    public class StoreTransactionsEODReport : CompanySettings
    {
        private readonly IBlobService _blobService;
        private readonly IEmailNotification _emailNotification;

        public StoreTransactionsEODReport(IBlobService blobService, IEmailNotification emailNotification)
        {
            _blobService = blobService;
            _emailNotification = emailNotification;
        }

        [FunctionName("StoreTransactions-EODReport")]
        //public async Task Run([TimerTrigger("0 0 16 * * *")] TimerInfo myTimer, ILogger log)
        public async Task Run([TimerTrigger(TimerTriggerConfig)] TimerInfo myTimer, ILogger log)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                bool getAll = Environment.GetEnvironmentVariable("retrieveAll") == "1";
                string dateToday = DateTime.Now.AddHours(8).AddDays(-1).ToString("dddd, dd MMMM yyyy"); // Wednesday, 24 January 2024
                string localDate = DateTime.Now.AddHours(8).AddDays(-1).ToString("yyyyMMdd"); // 20240124
                string formattedDateString = DateTimeOffset.ParseExact(localDate, "yyyyMMdd", null)
                    .AddHours(12)
                    .ToString("yyyy-MM-ddTHH:mm:ssZ"); // 2024-01-24T12:00:00Z

                //POS
                Blob.StoreTransactions BlobPoll = new(_blobService, getAll);
                string containerName = Environment.GetEnvironmentVariable("StoreTransactionContainer");
                stopwatch.Start();
                Model.StoreTransactions POSTransactions = BlobPoll.GetPOSTransactions(containerName, "POS", localDate);
                stopwatch.Stop();
                log.LogInformation($"POS Data Fetching - Time Elapsed: {stopwatch.Elapsed.TotalSeconds} seconds");

                if (!POSTransactions.Headers.Any())
                {
                    log.LogInformation($"No transaction found from POS on {dateToday}.");
                    return;
                }

                // D365
                FO.StoreTransactions FOAPI = new(int.Parse(CompanyId), getAll);
                stopwatch.Restart();
                Model.StoreTransactions StoreTransactions = await FOAPI.GetStoreTransactions(log, formattedDateString);
                stopwatch.Stop();
                log.LogInformation($"FO Data Fetching - Time Elapsed: {stopwatch.Elapsed.TotalSeconds} seconds");

                if (!StoreTransactions.Headers.Any())
                {
                    log.LogInformation($"No transaction found from D365 on {dateToday}.");
                    return;
                }

                List<string> attachmentFileNames = new()
                {
                    "StoreTransactionHeader",
                    "StoreTransactionLine",
                    "StoreTransactionDiscount",
                    "StoreTransactionPayment"
                };
                string subject = "[Transaction] End of Day Reports";
                string messageBody = "Good Day,\r\n\r\nKindly refer to the attached csv files containing the list of POS transactions.\n\nFor further assistance or support, please contact your administrator.";

                stopwatch.Restart();
                List<IEnumerable<Dictionary<string, object>>> attachmentDataCollection = new();
                if (getAll)
                {
                    attachmentDataCollection = CreateAllAttachmentData(POSTransactions, StoreTransactions);
                }
                else
                {
                    attachmentDataCollection = CreateAttachmentData(POSTransactions, StoreTransactions, localDate);
                }
                stopwatch.Stop();
                log.LogInformation($"Data Transformation - Time Elapsed: {stopwatch.Elapsed.TotalSeconds} seconds");

                _emailNotification.NotifyWithAttachments(subject, messageBody, attachmentDataCollection, attachmentFileNames, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private List<IEnumerable<Dictionary<string, object>>> CreateAttachmentData(Model.StoreTransactions POSTransactions, Model.StoreTransactions StoreTransactions, string localDate)
        {
            string formattedLocalDate = DateTime.ParseExact(localDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            var headersAttachmentData = POSTransactions.Headers
                .Where(header => header.TransactionDate == localDate)
                .Select(header =>
                {
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", header.Terminal},
                        {"Transaction Number", header.TransactionNumber},
                    };

                    if (!StoreTransactions.Headers.Any(storeHeader =>
                        storeHeader.ChannelReferenceId == header.ChannelReferenceId &&
                        storeHeader.Terminal == header.Terminal &&
                        storeHeader.TransactionNumber == header.TransactionNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                    {
                        TransactionDate = dictionary["Transaction Date"],
                        Store = dictionary["Store"],
                        Register = dictionary["Register"],
                        TransactionNumber = dictionary["Transaction Number"]
                    })
                .Select(group => group.First())
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var linesAttachmentData = POSTransactions.Lines
                .Select(line =>
                {
                    StoreTransactionsHeader header = GetHeader(line.TransactionNumber, line.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", line.Terminal},
                        {"Transaction Number", line.TransactionNumber},
                        {"Line Number", line.LineNumber},
                        {"Item ID", line.ItemId},
                        {"Net Amount", line.NetAmount},
                        {"Quantity", line.Quantity},
                        {"Item Sales Tax Group", line.ItemSalesTaxGroup}
                    };

                    if (!StoreTransactions.Lines.Any(storeLine =>
                        storeLine.Terminal == line.Terminal &&
                        storeLine.TransactionNumber == line.TransactionNumber &&
                        storeLine.LineNumber == line.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .Where(dictionary => ((string)dictionary["Transaction Date"]) == formattedLocalDate)
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var discountsAttachmentData = POSTransactions.Discounts
                .Select(discount =>
                {
                    StoreTransactionsHeader header = GetHeader(discount.TransactionNumber, discount.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", discount.Terminal},
                        {"Transaction Number", discount.TransactionNumber},
                        {"Line Number", discount.LineNumber},
                        {"Discount Amount", discount.DiscountAmount}
                    };

                    if (!StoreTransactions.Discounts.Any(storeDiscount =>
                        storeDiscount.Terminal == discount.Terminal &&
                        storeDiscount.TransactionNumber == discount.TransactionNumber &&
                        storeDiscount.LineNumber == discount.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .Where(dictionary => ((string)dictionary["Transaction Date"]) == formattedLocalDate)
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var paymentsAttachmentData = POSTransactions.Payments
                .Select(payment =>
                {
                    StoreTransactionsHeader header = GetHeader(payment.TransactionNumber, payment.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", payment.Terminal},
                        {"Transaction Number", payment.TransactionNumber},
                        {"Line Number", payment.LineNumber},
                        {"Amount Tendered", payment.AmountTendered }
                    };

                    if (!StoreTransactions.Payments.Any(storePayment =>
                        storePayment.Terminal == payment.Terminal &&
                        storePayment.TransactionNumber == payment.TransactionNumber &&
                        storePayment.LineNumber == payment.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .Where(dictionary => ((string)dictionary["Transaction Date"]) == formattedLocalDate)
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var attachmentDataCollection = new List<IEnumerable<Dictionary<string, object>>>
                {
                    headersAttachmentData.ToList(),
                    linesAttachmentData.ToList(),
                    discountsAttachmentData.ToList(),
                    paymentsAttachmentData.ToList()
                };

            return attachmentDataCollection;
        }

        private List<IEnumerable<Dictionary<string, object>>> CreateAllAttachmentData(Model.StoreTransactions POSTransactions, Model.StoreTransactions StoreTransactions)
        {
            var headersAttachmentData = POSTransactions.Headers
                .Select(header =>
                {
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", header.Terminal},
                        {"Transaction Number", header.TransactionNumber},
                    };

                    if (!StoreTransactions.Headers.Any(storeHeader =>
                        storeHeader.ChannelReferenceId == header.ChannelReferenceId &&
                        storeHeader.Terminal == header.Terminal &&
                        storeHeader.TransactionNumber == header.TransactionNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                })
                .Select(group => group.First())
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var linesAttachmentData = POSTransactions.Lines
                .Select(line =>
                {
                    StoreTransactionsHeader header = GetHeader(line.TransactionNumber, line.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", line.Terminal},
                        {"Transaction Number", line.TransactionNumber},
                        {"Line Number", line.LineNumber},
                        {"Item ID", line.ItemId},
                        {"Net Amount", line.NetAmount},
                        {"Quantity", line.Quantity},
                        {"Item Sales Tax Group", line.ItemSalesTaxGroup}
                    };

                    if (!StoreTransactions.Lines.Any(storeLine =>
                        storeLine.Terminal == line.Terminal &&
                        storeLine.TransactionNumber == line.TransactionNumber &&
                        storeLine.LineNumber == line.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var discountsAttachmentData = POSTransactions.Discounts
                .Select(discount =>
                {
                    StoreTransactionsHeader header = GetHeader(discount.TransactionNumber, discount.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", discount.Terminal},
                        {"Transaction Number", discount.TransactionNumber},
                        {"Line Number", discount.LineNumber},
                        {"Discount Amount", discount.DiscountAmount}
                    };

                    if (!StoreTransactions.Discounts.Any(storeDiscount =>
                        storeDiscount.Terminal == discount.Terminal &&
                        storeDiscount.TransactionNumber == discount.TransactionNumber &&
                        storeDiscount.LineNumber == discount.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var paymentsAttachmentData = POSTransactions.Payments
                .Select(payment =>
                {
                    StoreTransactionsHeader header = GetHeader(payment.TransactionNumber, payment.Terminal, POSTransactions.Headers);
                    string transactionDate = DateTime.ParseExact(header.TransactionDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    var dictionary = new Dictionary<string, object>
                    {
                        {"Transaction Date", transactionDate},
                        {"Store", header.ChannelReferenceId},
                        {"Register", payment.Terminal},
                        {"Transaction Number", payment.TransactionNumber},
                        {"Line Number", payment.LineNumber},
                        {"Amount Tendered", payment.AmountTendered}
                    };

                    if (!StoreTransactions.Payments.Any(storePayment =>
                        storePayment.Terminal == payment.Terminal &&
                        storePayment.TransactionNumber == payment.TransactionNumber &&
                        storePayment.LineNumber == payment.LineNumber))
                    {
                        dictionary["Status"] = "Missing in D365";
                    }
                    else
                    {
                        dictionary["Status"] = "Success";
                    }

                    return dictionary;
                })
                .GroupBy(dictionary => new
                {
                    TransactionDate = dictionary["Transaction Date"],
                    Store = dictionary["Store"],
                    Register = dictionary["Register"],
                    TransactionNumber = dictionary["Transaction Number"],
                    LineNumber = dictionary["Line Number"]
                })
                .Select(group => group.First())
                .OrderBy(dictionary => dictionary["Transaction Date"]);

            var attachmentDataCollection = new List<IEnumerable<Dictionary<string, object>>>
                {
                    headersAttachmentData.ToList(),
                    linesAttachmentData.ToList(),
                    discountsAttachmentData.ToList(),
                    paymentsAttachmentData.ToList()
                };

            return attachmentDataCollection;
        }

        private StoreTransactionsHeader GetHeader(string transactionNumber, string registerNumber, List<StoreTransactionsHeader> headers)
        {
            return headers.FirstOrDefault(header =>
                header.TransactionNumber == transactionNumber &&
                header.Terminal == registerNumber);
        }
    }
}
