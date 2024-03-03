using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using Azure.Storage.Queues;
using KTI.Moo.Extensions.Lazada.Model.Queue;
using System.Text;
using KTI.Moo.Extensions.Lazada.App.Queue.Helpers;
using KTI.Moo.Extensions.Lazada.Domain;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using KTI.Moo.Extensions.Lazada.Model;

namespace KTI.Moo.Extensions.Lazada.App.Queue.Receivers;

public class MessageCallback : CompanySettings
{
    private readonly IDistributedCache _cache;
    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Lazada.Model.ClientTokens, CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;

    public MessageCallback(IDistributedCache cache, Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Model.ClientTokens, CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain)
    {
        _cache = cache;
        _ClientTokenDomain = clientTokenDomain;
    }

    [FunctionName("MessageCallback")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "callback/lazada")] HttpRequest req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string method = req.Method.ToLower();

        // if query string contains a code, add an AuthCodeMessage to queue
        if (method.Equals("get") && req.Query.ContainsKey("code"))
        {

            log.LogInformation("Lazada callback Queue trigger function processed an authorization code.");

            var clientToken = _ClientTokenDomain.Create(req.Query["code"].ToString());

            if (clientToken != null && clientToken.CountryUserInfos.Length > 0)
            {
                var sellerid = clientToken.CountryUserInfos[0].SellerID;

                var Companyid = GetCompanyID(sellerid);

                log.LogInformation($"{Companyid}");

                if (Companyid == 0)
                {
                    _cache.SetString($"lazada_{clientToken.CountryUserInfos[0].Country}_{sellerid}", JsonConvert.SerializeObject(clientToken));

                    log.LogInformation($"{req.Query["code"]}");
                    log.LogInformation($"{sellerid} not yet register to a Company ID");

                    return new OkObjectResult("Lazada callback Queue trigger function processed an authorization code.");
                }

                _ClientTokenDomain.CompanyID = Companyid;

                var ChannelAppConfig = _ClientTokenDomain.GetbyLazadaSellerID(sellerid);
                var result = _ClientTokenDomain.UpdateToken(clientToken, ChannelAppConfig);

                if (result)
                {
                    log.LogInformation($"Update Token Success");
                }
                else
                {
                    log.LogInformation($"Update Token Failed");
                }
            }

            log.LogInformation($"{req.Query["code"]}");

            return new OkObjectResult("Lazada callback Queue trigger function processed an authorization code.");

        }

        // otherwise go through normal message validation process
        else if (method.Equals("post"))
        {
            // read message body and map to Message object
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Message message = JsonConvert.DeserializeObject<Message>(requestBody);
            var MessageString = JsonConvert.SerializeObject(message, Formatting.None);

            log.LogInformation("Lazada callback HTTP trigger function processed a message.");
            try
            {
                var signature = "";
                // check for Authorization header
                if (!req.Headers.ContainsKey("Authorization"))
                {
                    log.LogInformation("Processed request is missing Authorization header.");
                    return new UnauthorizedResult();
                }

                signature = req.Headers["Authorization"];


                if (string.IsNullOrWhiteSpace(config.AppKey) || string.IsNullOrWhiteSpace(config.AppSecret))
                {
                    throw new KeyNotFoundException("AppKey and/or AppSecret have not been set in Configuration.");
                }

                // verify signature from Authorization header
                string computed = MessageUtils.GetSignature(Encoding.ASCII.GetBytes(config.AppKey + requestBody), config.AppSecret);
                if (!computed.Equals(signature))
                {
                    // return 400 Bad Request if computed signature doesn't match provided signature
                    log.LogInformation("Processed request has malformed signature.");
                    return new BadRequestResult();
                }


                log.LogInformation("Processed request passes validation, adding to queue.");

                switch (message.Type)
                {
                    // add to lazada orders queue
                    case MessageTypes.TradeOrder:
                        {
                            log.LogInformation("Lazada callback Queue trigger function processed an order status notification.");
                            SendMessageToQueue(MessageString, OrderQueueName, ConnectionString);

                            log.LogInformation(MessageString);
                            // add message to queue and return 200 OK
                            return new OkResult();
                        }
                    case MessageTypes.FulfilmentOrder: //
                    case MessageTypes.ReverseOrder:
                        {
                            log.LogInformation("Lazada callback Queue trigger function processed an order status notification.");

                            return new OkResult();// new QueueResponse { Notification = message };
                        }
                    // add to lazada notifications queue
                    case MessageTypes.ProductQuality: // quality control status change
                    case MessageTypes.ProductCategory: // category tree is updated
                    case MessageTypes.ProductUpdate: // product is created or updated
                    case MessageTypes.ShallowStock: // low stock
                    case MessageTypes.AuthorizationToken: // authorization token expiry warning
                    case MessageTypes.SellerStatus: //
                    case MessageTypes.Promotion: //
                    case MessageTypes.ShortVideoState:
                        {

                            log.LogInformation("Lazada callback Queue trigger function processed a miscellaneous status notification.");
                            log.LogInformation($"{message.ToJson()}");
                            // Send email notif to clients
                            //   return new QueueResponse { Notification = message };
                            return new OkObjectResult(message);

                        }
                    default:
                        log.LogWarning($"Unhandled messe with unknown message type ({message.Type}):");
                        log.LogWarning(message.ToJson());
                        break;

                }


            }
            catch (System.Exception e)
            {
                log.LogError(e, e.Message);
                log.LogError("Exception occurred when processing the following message:");
                log.LogError(message.ToJson());
            }

        }

        // only allow GET with "code" in query string, or POST with json body
        // return 405 for everything else
        return new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);


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

    private int GetCompanyID(string sellerID)
    {
        //Add Company Here
        var SellerID3388 = Environment.GetEnvironmentVariable("Company_SellerID_3388");
        string[] Company3388 = SellerID3388.Split(',');

        if (Company3388.Any(sellerids => sellerids.Contains(sellerID)))
        {
            return 3388;
        }

        var SellerID3389 = Environment.GetEnvironmentVariable("Company_SellerID_3389");
        string[] Company3389 = SellerID3389.Split(',');

        if (Company3389.Any(sellerids => sellerids.Contains(sellerID)))
        {
            return 3389;
        }

        return 0;
    }

}
