using Azure.Storage.Queues;
using KTI.Moo.Extensions.Core.Domain.Queue;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Lazada;

public class OrderStatus : ChannelApps.Core.Domain.Dispatchers.IOrderStatus
{
    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {
        var FromCRMorderStatus = JsonConvert.DeserializeObject<Domain.Models.Sales.SalesOrderStatus>(messagequeue);

        var Lazadaorderstatus = new KTI.Moo.Extensions.Lazada.Model.OrderStatus()
        {
            kti_sourcesalesorderid = FromCRMorderStatus.kti_sourcesalesorderid,
            kti_sourcesalesorderitemids = FromCRMorderStatus.kti_sourcesalesorderitemids,
            orderstatus = FromCRMorderStatus.orderstatus,
            kti_orderstatus = FromCRMorderStatus.kti_orderstatus,
            packageid = FromCRMorderStatus.packageid,
            tracking_number = FromCRMorderStatus.tracking_number,
            shipment_provider = FromCRMorderStatus.shipment_provider,
            pdf_url = FromCRMorderStatus.pdf_url,
            kti_shipmentid = FromCRMorderStatus.kti_shipmentid,
            kti_salesorderid = FromCRMorderStatus.kti_salesorderid,
            kti_shipmentitemid = FromCRMorderStatus.kti_shipmentitemid,
            kti_salesorderitemid = FromCRMorderStatus.kti_salesorderitemid,
            kti_socialchannelorigin = FromCRMorderStatus.kti_socialchannelorigin,
            kti_cancelreason = FromCRMorderStatus.kti_cancelreason,
            laz_cancelReason = GetCancelReason(FromCRMorderStatus.kti_cancelreason),
            successful = FromCRMorderStatus.successful
        };


        return SendMessageToQueue(Lazadaorderstatus, QueueName, QueueConnectionString);
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

    private KTI.Moo.Extensions.Lazada.Model.CancelReason GetCancelReason(int kti_cancelreason)
    {
        return Extensions.Lazada.Model.CancelReason.SystemError;
    }
}
