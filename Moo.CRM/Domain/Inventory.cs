using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.CRM.Domain;

public class Inventory : Base.Domain.IInventory
{
    private readonly List<KTIDOMAIN.Models.AccountConfig> _listAccountConfig;
    private readonly List<KTIDOMAIN.Models.AccountConfig> _flowConfig;
    int _retryCounter = 0;
    private KTIDOMAIN.Models.CRMConfig _crmConfig;
    private string _accessToken;
    int _companyId = 0;

    public Inventory(int companyId)
    {
        _companyId = companyId;
        _listAccountConfig = KTIDOMAIN.Helper.GetListAccountConfig(_companyId);
        _crmConfig = KTIDOMAIN.Security.Authenticate.GetCRMConfig(_listAccountConfig);
        _flowConfig = KTIDOMAIN.Helper.GetListKeyByDelimiter(_listAccountConfig, "flow");
        _accessToken = KTIDOMAIN.Security.Authenticate.GetCRMAccessToken(_crmConfig).GetAwaiter().GetResult();
        _retryCounter = KTIDOMAIN.Helper.GetRetryCounter(_listAccountConfig);
    }

    public static async Task SendToChannelAppQueue(int companyid, string storecode, string channelorigin, string domainType, string QueueMessage, string ConnectionString)
    {

        var queuename = $"{companyid}-{channelorigin}-{storecode}-channelapp-{domainType}-dispatcher";
        await SendMessageToQueueAsync(QueueMessage, QueueName: queuename, ConnectionString);
    }


    private async Task<bool> SendMessageToQueue(object clientModel, string QueueName, string ConnectionString)
    {
        var Json = GetJsonForMessageQueue(clientModel);

        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync(Json);

        return true;
    }

    private static async Task<bool> SendMessageToQueueAsync(string Json, string QueueName, string ConnectionString)
    {
        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync(Json);

        return true;
    }


    private string GetJsonForMessageQueue(object clientModel)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);
        return Json;
    }



    public List<Origin.Core.Inventory.Models.Stock> GetInventoryStocks(List<string> productSKUList)
    {

        var inventoryManager = new Origin.Core.Inventory.InventoryManager(_crmConfig.crm_moo_db_connectionstring);

        return inventoryManager.GetCompanyStocks(_companyId, productSKUList).ToList();

    }



}
