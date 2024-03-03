using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models.Sales;
using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Helper;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Model.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.ChannelApps.Lazada.App.Receiver;

public class Order : CompanySettings
{

    private readonly KTI.Moo.Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<KTI.Moo.Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _ClientTokenDomain;

    public Order(Extensions.Core.Domain.Queue.IClientToken_Oauth2_SalesChannel<Extensions.Lazada.Model.ClientTokens, KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> clientTokenDomain)
    {
        _ClientTokenDomain = clientTokenDomain;
    }

    [FunctionName("Lazada_Order_ChannelApps_Receiver")]
    public void Run([QueueTrigger("moo-lazada-channelapps-order-receiver", Connection = "AzureQueueConnectionString")] KTI.Moo.Extensions.Lazada.Model.Queue.Message OrderLazadaMessage, ILogger log)
    {
        var ErrorOn = "Getting Cache Config:";
        try
        {

            log.LogInformation($"C# Queue trigger function processed: {JsonConvert.SerializeObject(OrderLazadaMessage)}");

            string LazadaOrderStatus = OrderLazadaMessage.Data.GetValueOrDefault("order_status");

            if (LazadaOrderStatus == "unpaid")
            {
                log.LogInformation($"Order is Unpaid");
                return;
            }

            var Companyid = GetCompanyID(OrderLazadaMessage.SellerId);

            ErrorOn = "Mapping Cache Config:";

            _ClientTokenDomain.CompanyID = Companyid;

            var ChannelConfig = _ClientTokenDomain.GetbyLazadaSellerID(OrderLazadaMessage.SellerId);

            if (ChannelConfig == null || string.IsNullOrWhiteSpace(ChannelConfig.kti_storecode))
            {
                log.LogInformation($"SellerId not setup in Channel Management: {OrderLazadaMessage.SellerId}");

                throw new Exception(OrderLazadaMessage.SellerId);
            }

            var LazadaConfig = new KTI.Moo.Extensions.Lazada.Service.Queue.Config()
            {
                AppKey = Environment.GetEnvironmentVariable("config_AppKey"),
                AppSecret = Environment.GetEnvironmentVariable("config_AppSecret"),
                companyid = ChannelConfig.kti_moocompanyid,
                defaultURL = ChannelConfig.kti_defaulturl,
                MaxPaginationInFinanceAPI = int.Parse(Environment.GetEnvironmentVariable("config_MaxPaginationInFinanceAPI")),
                MaxRetries = int.Parse(Environment.GetEnvironmentVariable("config_MaxRetries")),
                BaseSourceUrl = System.Environment.GetEnvironmentVariable("config_sourceURL"),
                Region = Environment.GetEnvironmentVariable("config_Region"),
                redisConnectionString = Environment.GetEnvironmentVariable("config_redisConnectionString"),
                SellerId = OrderLazadaMessage.SellerId
            };

            var ClientToken = new KTI.Moo.Extensions.Lazada.Model.ClientTokens()
            {
                AccessToken = ChannelConfig.kti_access_token,
                RefreshToken = ChannelConfig.kti_refresh_token,
                AccessExpiration = ChannelConfig.kti_access_expiration,
                RefreshExpiration = ChannelConfig.kti_refresh_expiration
            };

            if (OrderLazadaMessage.Data.TryGetValue("trade_order_id", out var orderId))
            {

                string buyerid = OrderLazadaMessage.Data.GetValueOrDefault("buyer_id");

                ErrorOn = "Getting Order:";
                var OrderDomain = new Extensions.Lazada.Domain.Queue.OrderTax(LazadaConfig, ClientToken);

                var LazadaOrder = new Extensions.Lazada.Model.OrderHeader();

                if (LazadaConfig.companyid == "3388" || LazadaConfig.companyid == "3389")
                {
                    LazadaOrder = OrderDomain.GetTax_Exclusive(orderId);
                }
                else
                {
                    LazadaOrder = OrderDomain.GetTax_Inclusive(orderId);
                }

                var ChannelAppsDTO = GetChannelAppsDTO(LazadaOrder, ChannelConfig, buyerid, LazadaOrderStatus);

                SendToStoreQueue(ChannelAppsDTO);

                log.LogInformation($"Success for CRM order to {ChannelConfig.kti_storecode}. Order Id: {orderId}. Buyer Id: {buyerid}.");
            }

        }
        catch (Exception ex)
        {
            log.LogError($"{ErrorOn} {ex.Message}");
            throw new Exception($"{ErrorOn} {ex.Message}");
        }
    }

    private bool SendToStoreQueue(Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel> channelApps)
    {
        var JSON = GetJson(channelApps);

        var Compress = JSON.ToBrotliAsync().GetAwaiter().GetResult().Result.Value;

        var ChannelAppConfig = channelApps.config;

        var queuename = $"{ChannelAppConfig.kti_moocompanyid}-lazada-{ChannelAppConfig.kti_storecode}-channelapp-order-receiver";

        SendMessageToQueueAsync(Compress, queuename, ChannelAppConfig.kti_azureconnectionstring);

        return true;
    }


    private Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel> GetChannelAppsDTO(Extensions.Lazada.Model.OrderHeader OrderFromMessage, CRM.Model.ChannelManagement.SalesChannel LazadaConfig, string buyerid, string orderstatus)
    {
        var CompanyID = int.Parse(LazadaConfig.kti_moocompanyid);
        OrderFromMessage.companyid = CompanyID;
        OrderFromMessage.order_status = orderstatus;
        OrderFromMessage.laz_customer.companyid = CompanyID;
        OrderFromMessage.laz_customer.kti_sourceid = buyerid;
        //    OrderFromMessage.laz_customer.laz_emailaddress = OrderFromMessage.emailaddress;
        OrderFromMessage.kti_socialchannel = LazadaConfig.kti_storecode;

        return new Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel>()
        {
            order = OrderFromMessage,
            customer = OrderFromMessage.laz_customer,
            config = LazadaConfig
        };

    }


    private static string GetJson(object models)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Extensions.Core.Helper.JSONSerializer.DontIgnoreResolver()
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return json;
    }

    public static bool SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
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

        var SellerID3392= Environment.GetEnvironmentVariable("Company_SellerID_3392");
        string[] Company3392 = SellerID3392.Split(',');

        if (Company3392.Any(sellerids => sellerids.Contains(sellerID)))
        {
            return 3392;
        }
        var SellerID3393 = Environment.GetEnvironmentVariable("Company_SellerID_3393");
        string[] Company3393 = SellerID3393.Split(',');

        if (Company3393.Any(sellerids => sellerids.Contains(sellerID)))
        {
            return 3393;
        }

        return 0;
    }
}
