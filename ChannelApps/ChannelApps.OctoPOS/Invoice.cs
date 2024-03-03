using KTI.Moo.Extensions.Core.Helper;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.ChannelApps.OctoPOS;

public class Invoice<GenericInvoice, GenericCustomer> : Core.Domain.IInvoice<GenericInvoice, CRM.Model.OrderBase, GenericCustomer> where GenericInvoice : CRM.Model.InvoiceBase where GenericCustomer : CRM.Model.CustomerBase
{

    private readonly string _companyId;
    private readonly string _connectionString;
    private readonly ILogger _log;


    public Invoice(string connectionString, string companyId, ILogger log)
    {
        _connectionString = connectionString;
        _companyId = companyId;
        _log = log;
    }


    public bool DefautProcess(string decodedJsonString)
    {
        return InvoiceProcess(decodedJsonString);
    }

    public bool WithCustomerProcess(string decodedJsonString)
    {
        var CustomerSuccess = CustomerProcess(decodedJsonString);

        if (CustomerSuccess)
        {
            return InvoiceProcess(decodedJsonString);
        }

        return false;
    }

    public bool With_Customer_Order_Process(string decodedJsonString)
    {
        throw new NotImplementedException();
    }

    public GenericCustomer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.OctoPOS.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);
        var CustomerModel = channelAppsInvoice.customer;


        if (CustomerModel is null)
        {
            throw new Exception("Customer invalid.");
        }

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            //   var clientModel = (GenericCustomer)Activator.CreateInstance(typeof(GenericCustomer), new object[] { record });

            var constructor = typeof(GenericCustomer).GetConstructor(new[] { typeof(Extensions.OctoPOS.Model.Customer) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(CustomerModel));
            var lambdaExpression = Expression.Lambda<Func<GenericCustomer>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();

            return clientModel;

        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor. {ex.Message}");

        }
    }

    public GenericInvoice GetClientModelInvoice(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.OctoPOS.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);
        var InvoiceModel = channelAppsInvoice.invoice;

        
        if (InvoiceModel is null)
        {
            throw new Exception("Invoice invalid.");
        }

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            //   var clientModel = (GenericInvoice)Activator.CreateInstance(typeof(GenericInvoice), new object[] { record });

            var constructor = typeof(GenericInvoice).GetConstructor(new[] { typeof(Extensions.OctoPOS.Model.Invoice) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(InvoiceModel));
            var lambdaExpression = Expression.Lambda<Func<GenericInvoice>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();

            return clientModel;

        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor. {ex.Message}");

        }
    }

    public CRM.Model.OrderBase GetClientModelOrder(string decodedJsonString)
    {
        throw new NotImplementedException();
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

    private bool InvoiceProcess(string decodedJsonString)
    {
        var Invoice = GetClientModelInvoice(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Invoice);
        var queueNameInvoice = $"{_companyId}{Core.Helpers.QueueName.Invoice}";

        return SendMessageToQueue(JSON, queueNameInvoice);
    }

    private bool CustomerProcess(string decodedJsonString)
    {
        var Customer = GetClientModelCustomer(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Customer);
        var queueNameCustomer = $"{_companyId}{Core.Helpers.QueueName.Customer}";

        return SendMessageToQueue(JSON, queueNameCustomer);
    }


 
}
