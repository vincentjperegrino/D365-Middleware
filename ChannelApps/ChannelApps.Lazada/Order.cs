using KTI.Moo.Extensions.Core.Helper;

namespace KTI.Moo.ChannelApps.Lazada;

public class Order<GenericOrder, GenericCustomer, GenericInvoice> : Core.Domain.IOrder<GenericOrder, GenericCustomer, GenericInvoice> where GenericOrder : CRM.Model.OrderBase where GenericCustomer : CRM.Model.CustomerBase where GenericInvoice : CRM.Model.InvoiceBase
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


    public GenericOrder GetClientModelOrder(string decodedJsonString)
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

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            // var clientModel = (GenericOrder)Activator.CreateInstance(typeof(GenericOrder), new object[] { OrderModel });


            var constructor = typeof(GenericOrder).GetConstructor(new[] { typeof(Extensions.Lazada.Model.OrderHeader) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(OrderModel));
            var lambdaExpression = Expression.Lambda<Func<GenericOrder>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();


            return clientModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor. {ex.Message}");
        }
    }


    public GenericCustomer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Lazada.Model.DTO.ChannelApps<CRM.Model.ChannelManagement.SalesChannel>>(decodedJsonString, JsonSettings);
        var CustomerModel = channelAppsInvoice.customer;

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            //  var clientModelCustomer = (GenericCustomer)Activator.CreateInstance(typeof(GenericCustomer), new object[] { CustomerModel });


            var constructor = typeof(GenericCustomer).GetConstructor(new[] { typeof(Extensions.Lazada.Model.Customer) });

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
        //No invoice in Lazada
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