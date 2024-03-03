using KTI.Moo.Extensions.Core.Service;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Moo.FO.Domain.Batch
{
    public class StoreTransactionBatch : Base.Domain.IStoreTransactionBatch
    {
        List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
        List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
        int _retryCounter = 0;
        private KTIDOMAIN.Models.FOConfig _foConfig;
        private string _accessToken;
        int _companyId = 0;

        public StoreTransactionBatch(int companyId)
        {
            _companyId = companyId;
            _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
            _foConfig = KTIDOMAIN.Security.Authenticate.GetFOConfig(_listAccountConfig);
            _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
            _accessToken = KTIDOMAIN.Security.Authenticate.GetFOAccessToken(_foConfig).GetAwaiter().GetResult();
            _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
        }

        public async Task<string> upsert(ILogger log, string batchId, string content, int requestCount)
        {
            int retryCount = 0;
            string responseContent = "";
            while (retryCount <= _retryCounter)
            {
                try
                {
                    List<string> locations = new List<string>();
                    HttpResponseMessage response = await SendBatchRequest(HttpMethod.Post, batchId, content);
                    responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(responseContent);
                    }
                    else
                    {
                        locations = ProcessResponse(responseContent);

                        if (locations.Count == requestCount)
                        {
                            break;
                        }

                        if (responseContent.Contains("error"))
                        {
                            List<string> extractedData = ExtractInnerErrorMessages(responseContent);

                            // Ignore record already exists error
                            if (CheckIfAllMessagesContainText(extractedData, "The record already exists"))
                            {
                                break;
                            }
                        }

                        if (locations.Count > 0)
                        {
                            string rollback = await delete(log, locations);
                        }
                        if (retryCount >= _retryCounter)
                        {
                            throw new Exception(responseContent);
                        }
                        retryCount++;
                    }
                }
                catch (Exception ex)
                {
                    if (retryCount >= _retryCounter)
                    {
                        log.LogError("FO Batch Upsert: An error has occured");
                        throw new Exception(ex.Message);
                    }
                    retryCount++;
                }
            }
            return responseContent;
        }

        private static bool CheckIfAllMessagesContainText(List<string> messages, string searchText)
        {
            return messages.All(message => message.Contains(searchText));
        }

        private static List<string> ExtractInnerErrorMessages(string inputText)
        {
            List<string> innerErrorMessages = new List<string>();

            // Define the regex pattern to match "innererror" messages
            string pattern = "\"innererror\":\\s*\\{\\s*\"message\":\\s*\"(.*?)\"";

            // Match the pattern using Regex
            MatchCollection matches = Regex.Matches(inputText, pattern, RegexOptions.Singleline);

            // Extract the matched messages
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string errorMessage = match.Groups[1].Value;
                    innerErrorMessages.Add(errorMessage);
                }
            }

            return innerErrorMessages;
        }

        private string FormatRequest(int counter, string timestamp, string apiURL)
        {
            string batchRequestTemplate = @"--batch_{timestamp}
Content-Type: multipart/mixed;boundary=changeset_{timestamp}{counter}

--changeset_{timestamp}{counter}
Content-Type: application/http
Content-Transfer-Encoding:binary

DELETE {api_url}?cross-company=true HTTP/1.1
Content-ID: {counter}
Accept: application/json;q=0.9, */*;q=0.1
OData-Version: 4.0
Content-Type: application/json
OData-MaxVersion: 4.0

--changeset_{timestamp}{counter}--

";

            string batchRequestFormat = batchRequestTemplate
                .Replace("{timestamp}", timestamp)
                .Replace("{counter}", counter.ToString())
                .Replace("{api_url}", apiURL);

            return batchRequestFormat;
        }

        private async Task<HttpResponseMessage> SendBatchRequest(HttpMethod methodType, string batchId, string content)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_foConfig.fo_resource_url + "/data/");
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = new HttpRequestMessage(methodType, "$batch");
                request.Content = new StringContent(content);
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.TryAddWithoutValidation("Content-Type", $"multipart/mixed; boundary={batchId}");

                return await httpClient.SendAsync(request);
            }
        }

        public List<string> ProcessResponse(string responseContent)
        {
            List<string> locations = responseContent.Split(new[] { "--batchresponse_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => GetLocation("--batchresponse_" + part))
                .Where(location => !string.IsNullOrEmpty(location))
                .ToList();

            return locations;
        }

        public async Task<string> delete(ILogger log, List<string> locations)
        {
            try
            {
                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                string batchId = $"batch_{timestamp}";
                string content = string.Join("", locations
                    .Select((location, index) =>
                    {
                        if (location.Contains("RetailTransactionPaymentLinesV2"))
                        {
                            location = Regex.Replace(location, "RetailTransactionPaymentLinesV2", "RetailTransactionPaymentLines");
                            location = Regex.Replace(location, @",Store='[A-Za-z0-9]*'", "");
                            return FormatRequest(index + 1, timestamp, location);
                        }
                        else
                        {
                            return FormatRequest(index + 1, timestamp, location);
                        }
                    }));


                HttpResponseMessage response = await SendBatchRequest(HttpMethod.Delete, batchId, content);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                log.LogError("FO Batch Delete: " + ex.Message);
                throw new Exception("FO Batch Delete: " + ex.Message);
            }
        }

        private string GetLocation(string changeSet)
        {
            int index = changeSet.IndexOf("Location:");
            if (index > 0)
            {
                var parts = changeSet.Substring(index).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                return parts[0].Replace("Location:", "").Trim();
            }
            return null;
        }
    }
}
