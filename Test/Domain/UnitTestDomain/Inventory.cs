using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestDomain;

public class Inventory
{
    private readonly Domain.Modules.Inventory _Domain;
    private readonly ILogger _logger;


    public Inventory()
    {
        _logger = Mock.Of<ILogger>();
        _Domain = new Domain.Modules.Inventory(_companyId: 3388);
    }

    [Fact]
    public async Task TestUpdateInventory()
    {
        var model = new Domain.Models.Items.Inventory()
        {
            product = "COF0024",
            warehouse = "WELF01",
            companyid = 3389,
            qtyonhand = 0
        };

        var result = await _Domain.upsert(model, _logger);

        Assert.True(result);

    }
}
