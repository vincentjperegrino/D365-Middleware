using KTI.Moo.Base.Domain.Queue;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace KTI.Moo.Extensions.Cyware.Services
{
    public class TeamsNotification : INotification
    {
        public bool Notify(string WebhookUrl, string Title, string Message, ILogger log)
        {
            if (string.IsNullOrWhiteSpace(WebhookUrl))
            {
                log.LogInformation("WebhookUrl invalid");
                throw new ArgumentException("WebhookUrl invalid", WebhookUrl);
            }

            if (string.IsNullOrWhiteSpace(Title))
            {
                log.LogInformation("Title invalid");
                throw new ArgumentException("Title invalid", Title);
            }

            if (string.IsNullOrWhiteSpace(Message))
            {
                log.LogInformation("Message invalid");
                throw new ArgumentException("Message invalid", Message);
            }

            var message = new
            {
                Title = Title,
                Text = ProcessMessage(Message)
            };


            var client = new HttpClient();
            var payload = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = client.PostAsync(WebhookUrl, payload).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                log.LogInformation("Message sent successfully.");
                return true;
            }
            else
            {
                log.LogError($"Error sending message. Status code: {response.StatusCode}");
                return false;
            }
        }

        private string ProcessMessage(string message)
        {
            // Parse the JSON string into a JsonDocument
            JsonDocument jsonDocument = JsonDocument.Parse(message);

            // Check if the JSON represents an array
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                // Start building the dynamic table HTML
                string dynamicTable = "<table>\n";

                // Create the table header row
                dynamicTable += "<tr>\n";
                foreach (JsonProperty property in jsonDocument.RootElement[0].EnumerateObject())
                {
                    dynamicTable += $"<th>{property.Name}</th>\n";
                }
                dynamicTable += "</tr>\n";

                // Create the table data rows for each array element
                foreach (JsonElement element in jsonDocument.RootElement.EnumerateArray())
                {
                    dynamicTable += "<tr>\n";
                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        dynamicTable += $"<td>{property.Value}</td>\n";
                    }
                    dynamicTable += "</tr>\n";
                }

                // Close the table
                dynamicTable += "</table>";

                return dynamicTable;
            }
            else
            {
                // If it's not an array, handle the single object as before
                string dynamicTable = "<table>\n";
                dynamicTable += "<tr>\n";
                foreach (JsonProperty property in jsonDocument.RootElement.EnumerateObject())
                {
                    dynamicTable += $"<th>{property.Name}</th>\n";
                }
                dynamicTable += "</tr>\n";

                dynamicTable += "<tr>\n";
                foreach (JsonProperty property in jsonDocument.RootElement.EnumerateObject())
                {
                    dynamicTable += $"<td>{property.Value}</td>\n";
                }
                dynamicTable += "</tr>\n";

                dynamicTable += "</table>";

                return dynamicTable;
            }
        }

    }
}
