using KTI.Moo.Extensions.Shopify.Service;
using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Domain
{
    public class Inventory : KTI.Moo.Extensions.Core.Domain.IInventory<Model.Inventory>
    {


        private readonly InventoryItemService _service;



        public Inventory(Config config)
        {
            _service = new(config.defaultURL, config.admintoken);

        }


        public Model.Inventory Get(int ProductID)
        {
            try
            {
                var InventoryDTO = _service.GetAsync(ProductID).GetAwaiter().GetResult();

                if (InventoryDTO is null)
                {
                    return new Model.Inventory();
                }

                var Inventory = new Model.Inventory(InventoryDTO);

                return Inventory;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by ProductID. {ex.Message}");
            }
        }

        public Model.Inventory Get(long ProductID)
        {
            try
            {
                var InventoryDTO = _service.GetAsync(ProductID).GetAwaiter().GetResult();

                if (InventoryDTO is null)
                {
                    return new Model.Inventory();
                }

                var Inventory = new Model.Inventory(InventoryDTO);

                return Inventory;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by ProductID. {ex.Message}");
            }
        }

        public Model.Inventory Get(string ProductSku)
        {
            try
            {
                var filter = new Model.DTO.InventoryCustomFilter
                {
                    sku = ProductSku
                };

                var DTOInventoryList = _service.ListAsync(filter).GetAwaiter().GetResult();



                if (DTOInventoryList is null)
                {
                    return new Model.Inventory();
                }

                var InventorySearchedByID = DTOInventoryList.Items.FirstOrDefault(inventory => inventory.Id.ToString() == ProductSku);

                if (InventorySearchedByID is null)
                {
                    return new Model.Inventory();
                }

                return new Model.Inventory(InventorySearchedByID);
            }

            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by ProductSku. {ex.Message}");
            }
        }


        public bool StockIn(Model.Inventory InventoryDetails)
        {

            throw new NotImplementedException();
        }

        public bool StockOut(Model.Inventory InventoryDetails)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.Inventory InventoryDetails)
        {
            try
            {
                var Inventory = new Model.DTO.Inventory(InventoryDetails);
                var result = _service.UpdateAsync(InventoryDetails.id, Inventory).GetAwaiter().GetResult();

                return result.Id == InventoryDetails.id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Update. {ex.Message}");
            }
        }
    }
}