using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain
{
    public class Inventory : IInventory<Model.Inventory>
    {

        private readonly IMagentoService _service;
        public const string APIDirectory = "/products";

        public Inventory(Config config, IDistributedCache cache)
        {
            this._service = new MagentoService(config, cache);
        }

        public Inventory(string defaultDomain, string redisConnectionString, string username, string password)
        {

            this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);

        }

        public Inventory(Config config)
        {
            this._service = new MagentoService(config);
        }



        public Inventory(IMagentoService service)
        {
            this._service = service;
        }

        public Model.Inventory Get(int ProductID)
        {
            if (ProductID <= 0)
            {
                throw new ArgumentException("Invalid ProductID", nameof(ProductID));
            }

            try
            {
                var ProductModel = GetFromProduct(ProductID);

                var returnInventory = ConvertToInventory(ProductModel.extension_attributes.stock_item);

                return returnInventory;
            }
            catch
            {
                return new Model.Inventory();
            }

        }

        public Model.Inventory Get(string ProductSku)
        {
            if (string.IsNullOrWhiteSpace(ProductSku))
            {
                throw new ArgumentException("Invalid ProductSku", nameof(ProductSku));
            }

            try
            {
                var ProductModel = GetFromProduct(ProductSku);

                var returnInventory = ConvertToInventory(ProductModel.extension_attributes.stock_item);

                return returnInventory;
            }
            catch
            {
                return new Model.Inventory();
            }
        }

        public bool Update(Model.Inventory InventoryDetails)
        {
            return Update(InventoryDetails.product, InventoryDetails.qtyonhand);
        }

        public bool Update(string FromDispatcherQueue)
        {
            var InventoryDetails = JsonConvert.DeserializeObject<KTI.Moo.Extensions.Magento.Model.Inventory>(FromDispatcherQueue);

            return Update(InventoryDetails.product, InventoryDetails.qtyonhand);
        }

        public bool Update(string ProductSku, double quantityonhand)
        {
            if (string.IsNullOrWhiteSpace(ProductSku))
            {
                var domain = _service.DefaultURL;

                var resonse = domain + " MagentoInventory, Method: Update. ProductSku is required";

                throw new ArgumentException(resonse);

            }

            if (quantityonhand < 0)
            {
                var domain = _service.DefaultURL;

                var resonse = domain + " MagentoInventory, Method: Update. quantityonhand can't be less than 0";

                throw new ArgumentException(resonse);

            }

            try
            {

                var ProductModel = GetFromProduct(ProductSku);

                if (ProductModel == null || string.IsNullOrWhiteSpace(ProductModel.sku))
                {
                    var resonse = " MagentoInventory, Method: Update. Product not exists";
                    throw new System.Exception(resonse);

                }

                ProductModel.extension_attributes.stock_item.qtyonhand = quantityonhand;

                return UpdateFromProduct(ProductModel);

            }
            catch (System.Exception ex)
            {
                var domain = _service.DefaultURL;

                throw new MagentoIntegrationException(domain, ex.Message);

            }


        }



        private Model.Product GetFromProduct(int ProductID)
        {
            Product MagentoProduct = new(_service);

            return MagentoProduct.Get(ProductID);
        }

        private Model.Product GetFromProduct(string ProductSku)
        {
            Product MagentoProduct = new(_service);

            return MagentoProduct.Get(ProductSku);
        }


        private bool UpdateFromProduct(Model.Product ProductModel)
        {
            Product MagentoProduct = new(_service);

            return MagentoProduct.Update(ProductModel);
        }


        public Model.Inventory ConvertToInventory(StockItem stockModel)
        {
            return JsonConvert.DeserializeObject<Model.Inventory>(JsonConvert.SerializeObject(stockModel));
        }

        public StockItem ConvertToStockItem(Model.Inventory InventoryModel)
        {
            return JsonConvert.DeserializeObject<StockItem>(JsonConvert.SerializeObject(InventoryModel));
        }

        public bool StockIn(Model.Inventory InventoryDetails)
        {
            throw new NotImplementedException();
        }

        public bool StockOut(Model.Inventory InventoryDetails)
        {
            throw new NotImplementedException();
        }

        public Model.Inventory Get(Model.Inventory InventoryDetails)
        {
            throw new NotImplementedException();
        }
    }
}
