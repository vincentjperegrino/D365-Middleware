using KTI.Moo.Extensions.Core.Helper;

namespace KTI.Moo.ChannelApps.OctoPOS;

public class Customer<GenericCustomer> : Core.Domain.ICustomer<GenericCustomer> where GenericCustomer : CRM.Model.CustomerBase
{

    private readonly string _companyId;
    private readonly string _connectionString;

    public Customer(string connectionString, string companyId)
    {
        _connectionString = connectionString;
        _companyId = companyId;
    }


    public bool DefautProcess(string decodedJsonString)
    {
        var ClientModel = GetClientModelCustomer(decodedJsonString);
        var JSON = GetJsonForMessageQueue(ClientModel);
        var queueNameCustomer = $"{_companyId}{Core.Helpers.QueueName.Customer}";

        return SendMessageToQueue(JSON, queueNameCustomer);
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
}
