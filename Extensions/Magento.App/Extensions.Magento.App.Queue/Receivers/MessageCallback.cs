using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using KTI.Moo.Extensions.SAP.Model.DTO.Customers;
using Domain;

namespace KTI.Moo.Extensions.Magento.App.Queue.Receivers;

public class MessageCallback : CompanySettings
{

    [FunctionName("MessageCallback")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "callback/magento/{storecode}")] HttpRequest req,
        string storecode,
        ILogger log)
    {
        if (storecode != StoreCode)
        {
            log.LogInformation($"Magento store code is not allowed {storecode}");
        }

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var Message = JsonConvert.DeserializeObject<Model.DTO.Webhooks.Message>(requestBody);

        log.LogInformation($"Message: {requestBody}");

        if (Message.CustomerID > 0)
        {
            log.LogInformation($"Customer Process");
            var CONFIG = ConfigHelper.Get();

            KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(CONFIG);

            var customer = CustomerDomain.Get(Message.CustomerID);

            var DTO = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
            {
                customer = customer
            };

            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
            };

             
            var json = JsonConvert.SerializeObject(DTO, Formatting.None, JsonSettings);
            log.LogInformation(json);

            var CompressionResults = await json.ToBrotliAsync();
            var CompressionResult = CompressionResults.Result.Value;

            SendMessageToQueue(CompressionResult, CustomerQueueName, ConnectionString);
            log.LogInformation($"Customer send to queue");

        }

        if (Message.OrderID > 0)
        {
            log.LogInformation($"Order Process");
            var CONFIG = ConfigHelper.Get();

            KTI.Moo.Extensions.Magento.Domain.Order ExtensionOrderDomain = new KTI.Moo.Extensions.Magento.Custom.NCCI.Domain.Order(CONFIG);
            KTI.Moo.Extensions.Magento.Domain.Customer CustomerDomain = new(CONFIG);

            var order = ExtensionOrderDomain.Get(Message.OrderID);

            log.LogInformation(order.increment_id);
            log.LogInformation($"Order Status: {order.order_status}");

            if(order.order_payment is not null)
            {
                log.LogInformation($"Order Payment Method: {order.order_payment.method}");
            }
            else
            {
                log.LogInformation($"No Payment Method");

                return new NotFoundResult();
            }

          
            if (AllowedBasedOnStatus(order))
            {     
                var CustomerModels = CustomerDomain.Get(order.customer_id);

                var Response = new KTI.Moo.Extensions.Magento.Model.DTO.ChannelApps()
                {
                    order = order,
                    customer = CustomerModels
                };

                var JsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new JSONSerializerHelper.DontIgnoreResolver()
                };

                var json = JsonConvert.SerializeObject(Response, Formatting.None, JsonSettings);

                log.LogInformation(json);
                var CompressionResults = await json.ToBrotliAsync();
                var CompressionResult = CompressionResults.Result.Value;

                SendMessageToQueue(CompressionResult, OrderQueueName, ConnectionString);
                log.LogInformation($"Order send to queue");
            }
            else
            {
                log.LogInformation($"Order not for Process");
            }


        }

        //dynamic data = JsonConvert.DeserializeObject(requestBody);

        return new OkObjectResult(requestBody);
    }

    private static bool AllowedBasedOnStatus(Model.Order order)
    {
        return order.order_payment.method == "cashondelivery"
                        || order.order_payment.method == "phoenix_cashondelivery"
                        || order.order_status == "processing"
                        || order.order_status == "complete";
    }

    private static bool SendMessageToQueue(string Json, string QueueName, string ConnectionString)
    {
        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();
        queueClient.SendMessage(Json);

        return true;
    }
}
