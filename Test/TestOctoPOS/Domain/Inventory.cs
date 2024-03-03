using KTI.Moo.Extensions.OctoPOS.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOctoPOS.Model;
using Xunit;

namespace TestOctoPOS.Domain;

public class Inventory : OctoPOSBase
{

    private readonly KTI.Moo.Extensions.OctoPOS.Domain.Inventory OctoPOSInventory;
    private readonly KTI.Moo.Extensions.OctoPOS.Domain.Product OctoPOSProduct;
    private readonly IDistributedCache _cache;

    public Inventory()
    {

        var services = new ServiceCollection();
        services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

        var provider = services.BuildServiceProvider();
        _cache = provider.GetService<IDistributedCache>();

        OctoPOSInventory = new(CongfigTest, _cache);
        OctoPOSProduct = new(CongfigTest, _cache);
    }


    [Fact]
    public void IFworkingStockIn()
    {
        string ProductCode = "ITAERO002A";

        var ProductModel = OctoPOSProduct.Get(ProductCode);
        var OriginalHasSerial = ProductModel.HasSerial;

        if (OriginalHasSerial == true)
        {
            ProductModel.HasSerial = false;
            OctoPOSProduct.Update(ProductModel);
        }

        KTI.Moo.Extensions.OctoPOS.Model.Inventory inventoryModel = new()
        {
            warehouse = "WECOM01",
            Remark = "Stockin by KTI-MOO",
            MovementItems = new()
            {
                new()
                {
                    qtyonhand = 3,
                    product = ProductCode
                }
            }

        };

        var response = OctoPOSInventory.StockIn(inventoryModel);

        if (OriginalHasSerial == true)
        {
            ProductModel.HasSerial = true;
            OctoPOSProduct.Update(ProductModel);
        }

        Assert.True(response);
    }

    [Fact]
    public void IFworkingStockOut()
    {
 
        string ProductCode = "ITAERO002A";

        var ProductModel = OctoPOSProduct.Get(ProductCode);
   //     var OriginalHasSerial = ProductModel.HasSerial;

        //if (OriginalHasSerial == true)
        //{
        //    ProductModel.HasSerial = false;
        //    OctoPOSProduct.Update(ProductModel);
        //}

        KTI.Moo.Extensions.OctoPOS.Model.Inventory inventoryModel = new()
        {
            warehouse = "WECOM01",
            Remark = "Stockin by KTI-MOO",
            MovementItems = new()
            {
                new()
                {
                    qtyonhand = 3,
                    product = ProductCode,
                    SerialList = new[]
                     {
                         "12","123"
                     }
                }
            }
        };

        var response = OctoPOSInventory.StockOut(inventoryModel);

        //if (OriginalHasSerial == true)
        //{
        //    ProductModel.HasSerial = true;
        //    OctoPOSProduct.Update(ProductModel);
        //}

        //log
        Assert.True(response);
    }

}
