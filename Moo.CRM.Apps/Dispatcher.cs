
using Azure.Storage.Queues;
using KTI.Moo.Base.Domain.Dispatchers;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.CRM.App;

public class Dispatcher : Helpers.CompanySettings
{

    private readonly KTI.Moo.Base.Domain.Dispatchers.IDefault<Model.ChannelManagement.SalesChannel> _dispatcher;
    private readonly Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.SalesChannel> _channelManagementDomain;

    public Dispatcher(KTI.Moo.Base.Domain.Dispatchers.IDefault<Model.ChannelManagement.SalesChannel> dispatcher,
                      KTI.Moo.Base.Domain.IChannelManagement<Model.ChannelManagement.SalesChannel> channelManagementDomain)
    {
        _dispatcher = dispatcher;
        _channelManagementDomain = channelManagementDomain;
    }

    [FunctionName("CRM-Dispatcher")]
    public void Run([QueueTrigger("%CompanyID%-dispatcher", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        var decodedString = Helpers.Decode.Base64(myQueueItem);

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        var company = Convert.ToInt32(Companyid);

        var ChannelList = _channelManagementDomain.GetChannelList();

        bool IsForProduction = IsProduction == "1";

        if (ChannelList is null || ChannelList.Count == 0 || !ChannelList.Any(ChannelList => ChannelList.kti_isproduction == IsForProduction))
        {
            throw new Exception("Invalid sales channel setup");
        }

        _dispatcher.DispatchProcess(ChannelList, IsForProduction, company, ConnectionString, decodedString, log).Wait();
    }

}
