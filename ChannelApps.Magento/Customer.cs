using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.Extensions.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Magento;

public class Customer<GenericCustomer> : ICustomer<GenericCustomer> where GenericCustomer : CRM.Model.CustomerBase
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
        return CustomerProcess(decodedJsonString);
    }

    public GenericCustomer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Magento.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);


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
    private bool CustomerProcess(string decodedJsonString)
    {
        var Customer = GetClientModelCustomer(decodedJsonString);
        var JSON = GetJsonForMessageQueue(Customer);
        var queueNameCustomer = $"{_companyId}{Core.Helpers.QueueName.Customer}";

        return SendMessageToQueue(JSON, queueNameCustomer);
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
