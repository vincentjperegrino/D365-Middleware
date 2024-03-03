using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Model
{
    public class BaseTest
    {
        public string connectionString = "UseDevelopmentStorage=true";
        public string companyID = "3388";
        public string productBarcodeQueueName = "moo-productbarcode-queue";
        public string discountQueueName = "moo-discount-queue";
        public string tenderQueueName = "moo-tender-queue";
        public string forexQueueName = "moo-forex-queue";
        public string priceHeaderQueueName = "moo-price-queue";
        public string productCategoryQueueName = "moo-productcategory-queue";
        public string productPriceQueueName = "moo-productprice-queue";
        public string productsQueueName = "moo-products-queue";
        public string registerQueueName = "moo-register-queue";
        public string storeQueueName = "moo-store-queue";
        public string discountProductQueueName = "moo-discountproduct-queue";

        public string GetProductBarcodeDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\ProductBarcodeQueue.json");
        }

        public string GetDiscountDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\DiscountQueue.json");
        }

        public string GetForExDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\ForExQueue.json");
        }

        public string GetPriceHeaderDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\PriceHeaderQueue.json");
        }

        public string GetProductCategoryDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\ProductCategoryQueue.json");
        }

        public string GetProductPriceDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\ProductPriceQueue.json");
        }

        public string GetProductsDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\ProductsQueue.json");
        }

        public string GetRegisterDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\RegisterQueue.json");
        }

        public string GetStoreDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\StoreQueue.json");
        }

        public string GetTenderDispatcher()
        {
            return File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\TenderQueue.json");
        }
    }
}
