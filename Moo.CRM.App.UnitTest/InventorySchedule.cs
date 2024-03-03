using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Linq;

namespace Moo.CRM.App.UnitTest;

public class InventorySchedule
{

    private readonly KTI.Moo.Base.Domain.Dispatchers.IInventory<KTI.Moo.CRM.Model.ChannelManagement.Inventory> _dispatcherDomain;
    private readonly KTI.Moo.Base.Domain.IChannelManagement<KTI.Moo.CRM.Model.ChannelManagement.Inventory> _GetDomain;
    private readonly ILogger _logger;

    public InventorySchedule()
    {
        _logger = Mock.Of<ILogger>();
        _dispatcherDomain = new KTI.Moo.CRM.Domain.Dispatchers.NCCI.Inventory();
        _GetDomain = new KTI.Moo.CRM.Domain.ChannelManagement.Inventory(3389);
    }


    [Fact]
    public async Task InsertToQueueInventoryBatchWorking()
    {
        var GetProductsInventory = _GetDomain.GetChannelList();

        string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

        var result = await _dispatcherDomain.DispatchBatchProcess(GetProductsInventory, ConnectionString, _logger);

        Assert.True(result);
    }

    [Fact]
    public async Task InsertToQueueInventoryWorking()
    {
        var GetProductsInventory = _GetDomain.Get("lazadatestncci");

        string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

        var result = await _dispatcherDomain.DispatchBatchProcessPerStore(GetProductsInventory, ConnectionString, _logger);

        Assert.True(result);
    }


}
