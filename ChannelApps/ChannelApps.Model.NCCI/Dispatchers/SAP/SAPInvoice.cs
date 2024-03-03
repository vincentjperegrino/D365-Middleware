using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP;
public class Invoice : IInvoiceToQueue
{

    public Invoice()
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

        KTI.Moo.Extensions.SAP.Domain.Invoice _SAPOrderDomain = new(Config);
        KTI.Moo.Extensions.SAP.Domain.Customer _SAPCustomerDomain = new(Config);





        var invoicemodel = JsonConvert.DeserializeObject<Domain.Models.Sales.DTO_InvoiceAndDetails>(messagequeue);
        KTI.Moo.Extensions.SAP.Model.Invoice SAPInvoice = new();

        SAPInvoice.DocEntry = invoicemodel.invoiceHeader.kti_sapdocentry;
        SAPInvoice.DocNum = invoicemodel.invoiceHeader.kti_sapdocnum;
        SAPInvoice.invoicenumber = invoicemodel.invoiceHeader.invoicenumber;
        SAPInvoice.DocDate = invoicemodel.invoiceHeader.kti_invoicedate.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates);
        SAPInvoice.DocDueDate = invoicemodel.invoiceHeader.kti_invoicedate.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates);
        SAPInvoice.customerid = GetCardCode(_SAPCustomerDomain, invoicemodel);
        SAPInvoice.description = string.IsNullOrWhiteSpace(invoicemodel.invoiceHeader.description) ? "CRM to SAP" : invoicemodel.invoiceHeader.description + ". CRM to SAP";
        SAPInvoice.Series = 4;
        SAPInvoice.ORNo = invoicemodel.invoiceHeader.name;
        //[OCTG] sap table for payment terms

        //COD
        //if (invoicemodel.invoiceHeader.kti_paymentmethod == 959080002)
        //{
        //    SAPInvoice.PaymentGroupCode = 3;
        //}

        ////Credit Card
        //if (invoicemodel.invoiceHeader.kti_paymenttermscode == 959080015)
        //{
        //    SAPOrder.PaymentGroupCode = 1;

        //}

        ////GCOD
        //if (invoicemodel.invoiceHeader.kti_paymenttermscode == 959_080_017)
        //{
        //    SAPOrder.PaymentGroupCode = 33;

        //}


        SAPInvoice.Address = new()
        {
            BillToStreet = invoicemodel.invoiceHeader.billto_line1,
            BillToZipCode = invoicemodel.invoiceHeader.billto_postalcode,
            BillToCity = invoicemodel.invoiceHeader.billto_city,
            BillToCountry = invoicemodel.invoiceHeader.billto_country,
            //          BillToState = ordermodel.order.billto_stateorprovince,

            ShipToStreet = invoicemodel.invoiceHeader.shipto_line1,
            ShipToZipCode = invoicemodel.invoiceHeader.shipto_postalcode,
            ShipToCity = invoicemodel.invoiceHeader.shipto_city,
            ShipToCountry = invoicemodel.invoiceHeader.shipto_country,
            //      ShipToState = ordermodel.order.shipto_stateorprovince
        };


        if (invoicemodel.invoiceHeader.kti_socialchannelorigin == 959080010)
        {
            SAPInvoice.Channel = "Magento";
            SAPInvoice.description = string.IsNullOrWhiteSpace(invoicemodel.invoiceHeader.description) ? "Magento to SAP" : invoicemodel.invoiceHeader.description + ". Magento to SAP";

        }

        if (invoicemodel.invoiceHeader.kti_socialchannelorigin == 959080011)
        {
            SAPInvoice.Channel = "Octopos";
            SAPInvoice.description = string.IsNullOrWhiteSpace(invoicemodel.invoiceHeader.description) ? "Magento to SAP" : invoicemodel.invoiceHeader.description + ". Magento to SAP";

        }

        SAPInvoice.InvoiceItems = invoicemodel.invoiceDetails.Select(details =>

        new KTI.Moo.Extensions.SAP.Model.InvoiceItem()
        {
            productid = details.productid,
            quantity = details.quantity,
            priceperunit = details.priceperunit,
            DiscountPercent = GetDiscountPercent(details)

        }).ToList();


        if (invoicemodel.invoiceHeader.freightamount > 0)
        {
            SAPInvoice.InvoiceItems.Add(new()
            {
                productid = "ITDELI004A",
                quantity = 1,
                priceperunit = invoicemodel.invoiceHeader.freightamount

            });

        }


        return SendMessageToQueue(SAPInvoice, QueueName, QueueConnectionString);
    }



    private static bool SendMessageToQueue(object clientModel, string QueueName, string ConnectionString)
    {
        var Json = GetJsonForMessageQueue(clientModel);

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



    private string GetCardCode(KTI.Moo.Extensions.SAP.Domain.Customer SAPCustomerDomain, Domain.Models.Sales.DTO_InvoiceAndDetails invoicemodel)
    {

        //Add Unique Key in Sales Channel Logic here
        var ExsistingCustomer = SAPCustomerDomain.GetByField("Cellular", invoicemodel.invoiceHeader.billto_telephone);

        if (!string.IsNullOrWhiteSpace(ExsistingCustomer.kti_sapbpcode))
        {
            return ExsistingCustomer.kti_sapbpcode;
        }

        ExsistingCustomer = SAPCustomerDomain.GetByField("EmailAddress", invoicemodel.invoiceHeader.emailaddress);

        if (!string.IsNullOrWhiteSpace(ExsistingCustomer.kti_sapbpcode))
        {
            return ExsistingCustomer.kti_sapbpcode;
        }

        return default;
    }


    private int GetDocEntry(Domain.Models.Sales.DTO_InvoiceAndDetails invoicemodel)
    {

        if (invoicemodel.invoiceHeader.kti_sapdocentry > 0)
        {
            return invoicemodel.invoiceHeader.kti_sapdocentry;
        }


        //Add Unique Key in Sales Channel Logic here
        //var ExsistingOrder = _SAPInvoiceDomain.GetByField("U_WebOrderNo", ordermodel.order.name);

        //if (ExsistingOrder.DocEntry > 0)
        //{
        //    return ExsistingOrder.DocEntry;
        //}


        return default;
    }

    private decimal GetDiscountPercent(Domain.Models.Sales.DTO_InvoiceDetails invoicedetailsmodel)
    {
        if (invoicedetailsmodel.manualdiscountamount > 0)
        {
            var Fullamountwithtax = (invoicedetailsmodel.priceperunit * (decimal)invoicedetailsmodel.quantity) + invoicedetailsmodel.tax;
            var Percent = 100;
            var DecimalPlaces = 4;
            return Math.Round((invoicedetailsmodel.manualdiscountamount / Fullamountwithtax) * Percent, DecimalPlaces);

        }

        return default;
    }

}
