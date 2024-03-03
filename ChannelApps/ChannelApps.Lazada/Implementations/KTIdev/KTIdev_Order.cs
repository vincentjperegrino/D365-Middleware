using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using KTI.Moo.ChannelApps.Model.NCCI.Receivers;
using KTI.Moo.Extensions.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Lazada.Implementations.KTIdev;

public class Order : Core.Domain.IOrder<ChannelApps.Model.KTIdev.Receivers.Order, ChannelApps.Model.KTIdev.Receivers.Customer, ChannelApps.Model.KTIdev.Receivers.Invoice> , IOrderForReceiver
{
    private readonly string _connectionString;
    private readonly string _companyId;

    public Order(string connectionString, string companyId)
    {
        _connectionString = connectionString;
        _companyId = companyId;
    }

    public bool DefautProcess(string decodedJsonString)
    {
        return OrderProcess(decodedJsonString);
    }

    public bool WithCustomerProcess(string decodedJsonString)
    {

        var SuccessCustomerSendQueue = CustomerProcess(decodedJsonString);

        if (SuccessCustomerSendQueue)
        {
            return OrderProcess(decodedJsonString);
        }

        return false;
    }


    public bool WithCustomer_Invoice_Process(string decodedJsonString)
    {
        //No invoice in Lazada
        throw new NotImplementedException();

    }
    public Model.KTIdev.Receivers.Customer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel>>(decodedJsonString, JsonSettings);
        var CustomerModel = channelAppsInvoice.customer;

        if (CustomerModel is null)
        {
            throw new Exception("Record invalid.");
        }

        var company = CustomerModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        return new Model.KTIdev.Receivers.Customer(CustomerModel);
    }

    public Model.KTIdev.Receivers.Invoice GetClientModelInvoice(string decodedJsonString)
    {
        throw new NotImplementedException();
    }

    public Model.KTIdev.Receivers.Order GetClientModelOrder(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel>>(decodedJsonString, JsonSettings);
        var OrderModel = channelAppsInvoice.order;

        if (OrderModel is null)
        {
            throw new Exception("Record invalid.");
        }

        var company = OrderModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        return new Model.KTIdev.Receivers.Order(OrderModel);

    }

    public string GetJsonForMessageQueue(object clientModel)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);

        return Json;
    }

    public bool SendMessageToQueue(string Json, string QueueName)
    {
        QueueClient queueClient = new QueueClient(_connectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExistsAsync().Wait();
        queueClient.SendMessage(Json.ToBrotliAsync().GetAwaiter().GetResult().Result.Value);

        return true;
    }


    private bool CustomerProcess(string decodedJsonString)
    {
        var Customer = GetClientModelCustomer(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Customer);
        var queueNameCustomer = $"{_companyId}{Core.Helpers.QueueName.Customer}";

        return SendMessageToQueue(JSON, queueNameCustomer);
    }


    private bool OrderProcess(string decodedJsonString)
    {
        var Order = GetClientModelOrder(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Order);
        var queueNameOrder = $"{_companyId}{Core.Helpers.QueueName.Order}";

        return SendMessageToQueue(JSON, queueNameOrder);
    }

    private bool InvoiceProcess(string decodedJsonString)
    {
        //No invoice in Lazada
        throw new NotImplementedException();
    }

}
