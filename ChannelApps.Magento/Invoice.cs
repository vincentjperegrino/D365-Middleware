

using KTI.Moo.Extensions.Core.Helper;

namespace KTI.Moo.ChannelApps.Magento;

public class Invoice<GenericInvoice, GenericOrder, GenericCustomer> : Core.Domain.IInvoice<GenericInvoice, GenericOrder, GenericCustomer> where GenericInvoice : CRM.Model.InvoiceBase where GenericOrder : CRM.Model.OrderBase where GenericCustomer : CRM.Model.CustomerBase
{

    private readonly string _companyId;
    private readonly string _connectionString;

    public Invoice(string connectionString, string companyId)
    {
        _connectionString = connectionString;
        _companyId = companyId;
    }

    public bool DefautProcess(string decodedJsonString)
    {
        return InvoiceProcess(decodedJsonString);
    }

    public bool WithCustomerProcess(string decodedJsonString)
    {
        var SuccessCustomerSendQueue = CustomerProcess(decodedJsonString);

        if (SuccessCustomerSendQueue)
        {
            return InvoiceProcess(decodedJsonString);
        }

        return false;
    }

    public bool With_Customer_Order_Process(string decodedJsonString)
    {
        var SuccessCustomerSendQueue = CustomerProcess(decodedJsonString);

        if (SuccessCustomerSendQueue)
        {
            var SuccessOrderSendQueue = OrderProcess(decodedJsonString);

            if (SuccessOrderSendQueue)
            {
                return InvoiceProcess(decodedJsonString);
            }
        }

        return false;
    }

    public GenericInvoice GetClientModelInvoice(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Magento.Model.DTO.ChannelApps>(decodedJsonString , JsonSettings);
        var InvoiceModel = channelAppsInvoice.invoice;

        if (InvoiceModel is null)
        {
            throw new Exception("Record invalid.");
        }

        var company = InvoiceModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            // var clientModel = (GenericInvoice)Activator.CreateInstance(typeof(GenericInvoice), new object[] { InvoiceModel });

            var constructor = typeof(GenericInvoice).GetConstructor(new[] { typeof(Extensions.Magento.Model.Invoice) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(InvoiceModel));
            var lambdaExpression = Expression.Lambda<Func<GenericInvoice>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();

            return clientModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor in Invoice. {ex.Message}");
        }
    }

    public GenericCustomer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Magento.Model.DTO.ChannelApps>(decodedJsonString,JsonSettings);


        var CustomerModel = channelAppsInvoice.customer;

        var company = CustomerModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }


        try
        {
            //     Model Must Have a Constructor for mapping queue records to Client Models
          //  var clientModelCustomer = (GenericCustomer)Activator.CreateInstance(typeof(GenericCustomer), new object[] { CustomerModel });


            var constructor = typeof(GenericCustomer).GetConstructor(new[] { typeof(Extensions.Magento.Model.Customer) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(CustomerModel));
            var lambdaExpression = Expression.Lambda<Func<GenericCustomer>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();


            return clientModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor in Customer. {ex.Message}");
        }
    }

    public GenericOrder GetClientModelOrder(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Magento.Model.DTO.ChannelApps>(decodedJsonString,JsonSettings);
        var OrderModel = channelAppsInvoice.order;

        var company = OrderModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        try
        {
            //Model Must Have a Constructor for mapping queue records to Client Models
            //  var clientModelOrder = (GenericOrder)Activator.CreateInstance(typeof(GenericOrder), new object[] { OrderModel });

            var constructor = typeof(GenericOrder).GetConstructor(new[] { typeof(Extensions.Magento.Model.Order) });

            var constructorExpression = Expression.New(constructor, Expression.Constant(OrderModel));
            var lambdaExpression = Expression.Lambda<Func<GenericOrder>>(constructorExpression);
            var createFunc = lambdaExpression.Compile();
            var clientModel = createFunc();

            return clientModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Invalid Constructor in Order. {ex.Message}");
        }
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


    private bool OrderProcess(string decodedJsonString)
    {
        var Order = GetClientModelOrder(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Order);
        var queueNameOrder = $"{_companyId}{Core.Helpers.QueueName.Order}";

        return SendMessageToQueue(JSON, queueNameOrder);
    }


}
