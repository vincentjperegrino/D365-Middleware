using Azure.Storage.Queues.Models;
using KTI.Moo.Base.Domain.Queue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Domain.Queue
{
    public class PoisonNotification : Notification , IPoisonNotification
    {
        //public bool Notify(string WebhookUrl, string Title, string Message, ILogger log)
        //{
        //    if (string.IsNullOrWhiteSpace(WebhookUrl))
        //    {
        //        log.LogInformation("WebhookUrl invalid");
        //        throw new ArgumentException("WebhookUrl invalid", WebhookUrl);
        //    }

        //    if (string.IsNullOrWhiteSpace(Title))
        //    {
        //        log.LogInformation("Title invalid");
        //        throw new ArgumentException("Title invalid", Title);
        //    }

        //    if (string.IsNullOrWhiteSpace(Message))
        //    {
        //        log.LogInformation("Message invalid");
        //        throw new ArgumentException("Message invalid", Message);
        //    }

        //    var message = new
        //    {
        //        Title = Title,
        //        Text = Message
        //    };

        //    var client = new HttpClient();
        //    var payload = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        //    var response = client.PostAsync(WebhookUrl, payload).GetAwaiter().GetResult();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        log.LogInformation("Message sent successfully.");
        //        return true;
        //    }
        //    else
        //    {
        //        log.LogError($"Error sending message. Status code: {response.StatusCode}");
        //        return false;
        //    }
        //}

    }
}
