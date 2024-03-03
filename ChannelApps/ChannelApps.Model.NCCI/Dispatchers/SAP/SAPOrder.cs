using Azure.Storage.Queues;
using KTI.Moo.Base.Helpers;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP;

public class Order : IOrderToQueue
{

    public Order()
    {

    }

    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {

        var configFromQueueObject = JsonConvert.DeserializeObject<JObject>(messagequeue);

        var configFromQueueSalesChannelObject = configFromQueueObject[KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname];

        var saleschannel = JsonConvert.SerializeObject(configFromQueueSalesChannelObject);

        var configFromQueueSalesChannel = JsonConvert.DeserializeObject<CRM.Model.ChannelManagement.SalesChannel>(saleschannel);

        KTI.Moo.Extensions.SAP.Service.Config Config = new()
        {
            defaultURL = configFromQueueSalesChannel.kti_defaulturl,
            redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False",
            password = configFromQueueSalesChannel.kti_password,
            username = configFromQueueSalesChannel.kti_username,
            companyDB = configFromQueueSalesChannel.kti_databasename,
            companyid = CompanyID,
        };

        KTI.Moo.Extensions.SAP.Domain.Order _SAPOrderDomain = new(Config);
        KTI.Moo.Extensions.SAP.Domain.Customer _SAPCustomerDomain = new(Config);

        var ordermodel = JsonConvert.DeserializeObject<Domain.Models.Sales.DTO_OrderAndDetails>(messagequeue);

        KTI.Moo.Extensions.SAP.Model.Order SAPOrder = new();

        if (ordermodel.order.kti_sapdocentry == 0)
        {
            SAPOrder.salesorderid = ordermodel.order.salesorderid;
        }


        SAPOrder.DocEntry = GetDocEntry(_SAPOrderDomain, ordermodel);
        SAPOrder.DocNum = ordermodel.order.kti_sapdocnum;

        SAPOrder.DocDate = ordermodel.order.datefulfilled.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates);
        SAPOrder.DocDueDate = ordermodel.order.datefulfilled.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates);

        if (SAPOrder.DocEntry > 0)
        {
            SAPOrder.DocDate = default;
            SAPOrder.DocDueDate = default;
        }


        SAPOrder.customerid = GetCardCode(_SAPCustomerDomain, ordermodel);

        if (string.IsNullOrWhiteSpace(SAPOrder.customerid))
        {
            throw new Exception("Customer not existing in SAP");
        }

        SAPOrder.description = string.IsNullOrWhiteSpace(ordermodel.order.description) ? "CRM to SAP" : ordermodel.order.description + ". CRM to SAP";

        SAPOrder.WebOrderNo = ordermodel.order.name;
        SAPOrder.statuscode = ordermodel.order.statuscode;
        SAPOrder.kti_paymenttermscode = ordermodel.order.kti_paymenttermscode;

        SAPOrder.kti_orderstatus = ordermodel.order.kti_orderstatus;

        //[OCTG] sap table for payment terms
        //COD
        if (ordermodel.order.kti_paymenttermscode == 959_080_002)
        {
            SAPOrder.PaymentGroupCode = 3;
        }

        //Credit Card
        if (ordermodel.order.kti_paymenttermscode == 959_08_0015 || ordermodel.order.kti_paymenttermscode == 959_080_016 || ordermodel.order.kti_paymenttermscode == 959_080_044)
        {
            SAPOrder.PaymentGroupCode = 1;
        }

        //GCOD
        if (ordermodel.order.kti_paymenttermscode == 959_080_017)
        {
            SAPOrder.PaymentGroupCode = 33;
        }

        SAPOrder.Address = new()
        {
            BillToStreet = ordermodel.order.billto_line1.TrimFirst100Characters(),
            BillToZipCode = ordermodel.order.billto_postalcode,
            BillToCity = ordermodel.order.billto_city,
            BillToCountry = ordermodel.order.billto_country == "Philippines" ? "PH" : ordermodel.order.billto_country,
            //          BillToState = ordermodel.order.billto_stateorprovince,

            ShipToStreet = ordermodel.order.shipto_line1.TrimFirst100Characters(),
            ShipToZipCode = ordermodel.order.shipto_postalcode,
            ShipToCity = ordermodel.order.shipto_city,
            ShipToCountry = ordermodel.order.shipto_country == "Philippines" ? "PH" : ordermodel.order.shipto_country,
            //      ShipToState = ordermodel.order.shipto_stateorprovince
        };

        if (ordermodel.order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_magento)
        {
            SAPOrder.Series = 74;
            SAPOrder.Channel = "Magento";
            SAPOrder.description = string.IsNullOrWhiteSpace(ordermodel.order.description) ? "Magento to SAP" : ordermodel.order.description + ". Magento to SAP";
        }

        if (ordermodel.order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos)
        {
            SAPOrder.Channel = "Octopos";
            SAPOrder.description = string.IsNullOrWhiteSpace(ordermodel.order.description) ? "Octopos to SAP" : ordermodel.order.description + ". Octopos to SAP";
        }

        var counter = 0;

        SAPOrder.OrderItems = ordermodel.orderdetails.Where(details => string.IsNullOrWhiteSpace(details.parentbundleid))
        .Select(details =>
        {
            var currentcount = counter;

            counter++;

            return new KTI.Moo.Extensions.SAP.Model.OrderItem()
            {
                LineNum = currentcount,
                productid = details.productid,
                quantity = details.quantity,
                priceperunit = details.priceperunit,
                WarehouseCode = KTI.Moo.Extensions.SAP.Helper.Order.WarehouseCode.ECOMM,
                DiscountPercent = GetDiscountPercent(details)

            };

        }).ToList();


        if (ordermodel.order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_lazada)
        {
            SAPOrder.PaymentGroupCode = 3; //only COD payment group for lazada Orders. NCCI custom mapping 
            SAPOrder.Series = 74;
            SAPOrder.Channel = "Lazada";
            SAPOrder.description = string.IsNullOrWhiteSpace(ordermodel.order.description) ? "Lazada to SAP" : ordermodel.order.description + ". Lazada to SAP";
            SAPOrder.WebOrderNo = GetLastTenDigits(ordermodel.order.name);
            SAPOrder.SORemarks = ordermodel.order.name;
            SAPOrder.NumAtCard = ordermodel.order.name; // Customer Ref Number


            counter = 0;
            SAPOrder.OrderItems = SAPOrder.OrderItems.GroupBy(grp => new
            {
                grp.productid,
                grp.DiscountPercent
            })
                                                     .Select(items =>
                                                     {
                                                         var currentcount = counter;
                                                         counter++;
                                                         return new KTI.Moo.Extensions.SAP.Model.OrderItem()
                                                         {
                                                             LineNum = currentcount,
                                                             productid = items.Key.productid,
                                                             quantity = items.Sum(item => item.quantity),
                                                             priceperunit = items.FirstOrDefault().priceperunit,
                                                             WarehouseCode = KTI.Moo.Extensions.SAP.Helper.Order.WarehouseCode.LAZADA,
                                                             DiscountPercent = items.Key.DiscountPercent
                                                         };

                                                     }).ToList();
        }


        var lastcount = counter;

        if (ordermodel.order.freightamount > 0 && ordermodel.order.kti_socialchannelorigin != CRM.Helper.ChannelOrigin.OptionSet_lazada)
        {
            SAPOrder.OrderItems.Add(new()
            {
                LineNum = lastcount,
                productid = "ITDELI004A",
                quantity = 1,
                priceperunit = ordermodel.order.freightamount,
                WarehouseCode = ordermodel.order.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_lazada ? KTI.Moo.Extensions.SAP.Helper.Order.WarehouseCode.LAZADA : KTI.Moo.Extensions.SAP.Helper.Order.WarehouseCode.ECOMM

            });

        }
        return SendMessageToQueue(SAPOrder, QueueName, QueueConnectionString);
    }



    private static bool SendMessageToQueue(object clientModel, string QueueName, string ConnectionString)
    {
        var Json = GetJsonForMessageQueue(clientModel);

        return SendMessageToQueue(Json, QueueName, ConnectionString);
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

    private static string GetJsonForMessageQueue(object clientModel)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);
        return Json;
    }


    private string GetCardCode(KTI.Moo.Extensions.SAP.Domain.Customer SAPCustomerDomain, Domain.Models.Sales.DTO_OrderAndDetails ordermodel)
    {
        //Add Unique Key in Sales Channel Logic here

        if (!string.IsNullOrWhiteSpace(ordermodel.order.emailaddress))
        {
            var ExsistingCustomer = SAPCustomerDomain.GetByField("EmailAddress", ordermodel.order.emailaddress);

            if (!string.IsNullOrWhiteSpace(ExsistingCustomer.kti_sapbpcode))
            {
                return ExsistingCustomer.kti_sapbpcode;
            }

        }

        if (!string.IsNullOrWhiteSpace(ordermodel.order.billto_telephone))
        {
            var ExsistingCustomer = SAPCustomerDomain.GetByField("Cellular", ordermodel.order.billto_telephone);

            if (!string.IsNullOrWhiteSpace(ExsistingCustomer.kti_sapbpcode))
            {
                return ExsistingCustomer.kti_sapbpcode;
            }
        }

        return default;
    }

    private int GetDocEntry(KTI.Moo.Extensions.SAP.Domain.Order SAPOrderDomain, Domain.Models.Sales.DTO_OrderAndDetails ordermodel)
    {

        if (ordermodel.order.kti_sapdocentry > 0)
        {
            return ordermodel.order.kti_sapdocentry;
        }

        //Add Unique Key in Sales Channel Logic here
        var ExsistingOrder = SAPOrderDomain.GetByField("U_WebOrderNo", ordermodel.order.name);

        if (ExsistingOrder.DocEntry <= 0)
        {
            return default;
        }

        return ExsistingOrder.DocEntry;

    }


    private decimal GetDiscountPercent(Domain.Models.Sales.DTO_OrderDetails orderdetailsmodel)
    {
        if (orderdetailsmodel.manualdiscountamount <= 0)
        {
            return default;
        }

        var taxMultiplier = (decimal)1.12;

        decimal pricewithTax = orderdetailsmodel.priceperunit * taxMultiplier;

        var Fullamountwithtax = (pricewithTax * (decimal)orderdetailsmodel.quantity);
        var Percent = 100;
        var DecimalPlaces = 4;

        return Math.Round((orderdetailsmodel.manualdiscountamount / Fullamountwithtax) * Percent, DecimalPlaces);

    }


    private string GetLastTenDigits(string WebOrderNumber)
    {
        if (string.IsNullOrWhiteSpace(WebOrderNumber))
        {
            return default;
        }

        var WebOrderNumber_15 = WebOrderNumber;

        var TenDigits = 10;

        var WebOrderNumber_10 = WebOrderNumber_15[^TenDigits..];

        return WebOrderNumber_10;

    }



}
