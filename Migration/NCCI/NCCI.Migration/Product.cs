using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace NCCI.Migration.Magento;

public class Product
{

    [Fact]
    public async Task MigrationProduct()
    {
        Domain.Modules.Product PRODUCTdomain = new(3389);

        var result = await PRODUCTdomain.getall_Inventory();

        var results = JsonConvert.DeserializeObject<dynamic>(result);
     
        foreach (var item in results.value)
        {
            var ProductList = new Domain.Models.Items.Products(item);

           await PRODUCTdomain.ToMigrationQueueInventory(ProductList);
        }

        Assert.True(true);
    }

    [Fact]
    public async Task TestMigrationProduct()
    {
        Domain.Modules.Product PRODUCTdomain = new(3389);

        var result = await PRODUCTdomain.getall();

        var results = JsonConvert.DeserializeObject<dynamic>(result);

        List<Domain.Models.Items.Products> ProductList = new();

        foreach (var item in results.value)
        {
            var products = new Domain.Models.Items.Products(item);
            products.pricelevelid = item._pricelevelid_value;

            ProductList.Add(products);
        }

        Domain.Modules.ProductPriceLevel PRODUCTpricelevldomain = new(3389);


        foreach (var item in ProductList)
        {
            try
            {
                var pricelevelresults = await PRODUCTpricelevldomain.getSKU_pricelevel(item.productnumber, item.pricelevelid);
                var priceresults = JsonConvert.DeserializeObject<dynamic>(pricelevelresults);

                foreach (var itemss in priceresults.value)
                {
                    var leveproducts = new Domain.Models.Items.ProductPriceLevel(itemss);
                    item.price = leveproducts.amount;
                    break;
                }
            }
            catch
            {
                item.price = 1;
            }

            if (item.price == 0)
            {
                item.price = 1;
            }

            await PRODUCTdomain.ToMigrationQueue(item);
        }

        Assert.True(true);
    }



    [Fact]
    public async Task TestPriceLevel()
    {
        Domain.Modules.ProductPriceLevel domain = new(3389);

        var result = await domain.getall();

        //    var results = JsonConvert.DeserializeObject<dynamic>(result);

        //List<Domain.Models.Items.Products> ProductList = new();

        //foreach (var item in results.value)
        //{
        //    ProductList.Add(new Domain.Models.Items.Products(item));
        //}


        //foreach (var item in ProductList.Take(10))
        //{
        //    await PRODUCTdomain.ToTestMigrationQueue(item);
        //}

        Assert.True(true);
    }




}
