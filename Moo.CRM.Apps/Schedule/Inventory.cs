using KTI.Moo.Base.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.CRM.App.Schedule;

public class Inventory : Helpers.CompanySettings
{

    private readonly Base.Domain.Dispatchers.IInventory<KTI.Moo.CRM.Model.ChannelManagement.Inventory> _dispatcherInventory;
    private readonly Base.Domain.IChannelManagementInventory<KTI.Moo.CRM.Model.ChannelManagement.Inventory> _channelManagementInventoryDomain;

    public Inventory(Base.Domain.Dispatchers.IInventory<Model.ChannelManagement.Inventory> dispatcherInventory,
                     Base.Domain.IChannelManagementInventory<Model.ChannelManagement.Inventory> channelManagementInventoryDomain)
    {
        _dispatcherInventory = dispatcherInventory;
        _channelManagementInventoryDomain = channelManagementInventoryDomain;
    }


    //1:00am in PHT 
    [FunctionName("Schedule_Inventory")]
    public void Run([TimerTrigger("0 0 17 * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var ChannelList = _channelManagementInventoryDomain.GetChannelList();

            bool IsForProduction = IsProduction == "1";

            ChannelList = ChannelList.Where(channel => channel.kti_moocompanyid == Companyid && channel.kti_isproduction == IsForProduction).ToList();

            Process(ChannelList, ConnectionString, log).Wait();

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new Exception(ex.Message);
        }

    }

    private async Task<bool> Process(List<CRM.Model.ChannelManagement.Inventory> ChannelList, string ConnectionString, ILogger log)
    {
        return await _dispatcherInventory.DispatchBatchProcess(ChannelList, ConnectionString, log);

    }

}
