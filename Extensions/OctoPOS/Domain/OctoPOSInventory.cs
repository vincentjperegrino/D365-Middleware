using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.OctoPOS.Exception;
using KTI.Moo.Extensions.OctoPOS.Model;
using KTI.Moo.Extensions.OctoPOS.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Domain;

public class Inventory : IInventory<Model.Inventory>
{

    private readonly IOctoPOSService _service;
    public const string APIDirectory = "/inventory";

    public Inventory(Config config, IDistributedCache cache)
    {
        this._service = new OctoPOSService(cache, config);
    }


    public Inventory(IOctoPOSService service)
    {
        this._service = service;
    }

    public Model.Inventory Get(int ProductID)
    {
        throw new NotImplementedException();
    }

    public Model.Inventory Get(string ProductSku)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Location Code and Products. 1 Product only
    /// </summary>
    /// <param name="InventoryDetails"></param>
    /// <returns>Available quantity from the location</returns>
    public double GetQuantity(Model.Inventory InventoryDetails)
    {
        if (InventoryDetails is null)
        {
            throw new ArgumentNullException(nameof(InventoryDetails));
        }

        try
        {
            string productid = InventoryDetails.product;

            if (InventoryDetails.MovementItems is not null && InventoryDetails.MovementItems.Count > 0)
            {
                productid = InventoryDetails.MovementItems.FirstOrDefault().product;
            }

            string path = $"{APIDirectory}/location/{InventoryDetails.warehouse}/product/{productid}";

            bool isAuthenticated = true;
            string method = "GET";

            var response = _service.ApiCall(path, method, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnInventoryData = JsonConvert.DeserializeObject<Model.DTO.Inventories.GetQuantity>(response, settings);

            return ReturnInventoryData.Quantity;

        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + $"{APIDirectory}/location/{InventoryDetails.warehouse}/product/{InventoryDetails.product}";
            var classname = "OctoPOSInventory, Method: GetQuantity";
            throw new OctoPOSIntegrationException(domain, classname, ex.Message);
        }
    }

    public bool StockIn(Model.Inventory InventoryDetails)
    {
        var result = StockMovement(InventoryDetails, Helper.InventoryMovementType.StockIN);
        return result;
    }

    public bool StockOut(Model.Inventory InventoryDetails)
    {
        var result = StockMovement(InventoryDetails, Helper.InventoryMovementType.StockOUT);
        return result;

    }

    private bool StockMovement(Model.Inventory InventoryDetails, string movement)
    {
        if (InventoryDetails is null)
        {
            throw new ArgumentNullException(nameof(InventoryDetails));
        }

        try
        {

            string path = "/stockMovement/create";
            bool isAuthenticated = true;
            string method = "POST";

            InventoryDetails.MovementType = movement;

            var StocksMovementDTO = new Model.DTO.Inventories.StocksMovement();
            StocksMovementDTO.MovementItems = InventoryDetails.MovementItems;
            InventoryDetails.MovementItems = null;
            StocksMovementDTO.StockMovement = InventoryDetails;

            var stringContent = GetContent(StocksMovementDTO);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnInventoryData = JsonConvert.DeserializeObject<Model.DTO.Inventories.StocksMovement>(response, settings);

            if (ReturnInventoryData is null || ReturnInventoryData.StockMovement is null || string.IsNullOrWhiteSpace(ReturnInventoryData.StockMovement.StockMovementNumber))
            {
                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + "/stockMovement/create";
            var classname = "OctoPOSInventory, Method: StockMovement";
            throw new OctoPOSIntegrationException(domain, classname, ex.Message);
        }
    }

    public bool Update(Model.Inventory InventoryDetails)
    {
        if (InventoryDetails is null)
        {
            throw new ArgumentNullException(nameof(InventoryDetails));
        }

        try
        {
            var CurrentOctoPosQty = GetQuantity(InventoryDetails);

            //StockIn
            //       35                              30
            if (InventoryDetails.qtyonhand > CurrentOctoPosQty)
            {

                var difference = InventoryDetails.qtyonhand - CurrentOctoPosQty;

                InventoryDetails.qtyonhand = difference;

                return StockIn(InventoryDetails);
            }

            //StockOut  
            //       30                              35
            if (InventoryDetails.qtyonhand < CurrentOctoPosQty)
            {
                var difference = CurrentOctoPosQty - InventoryDetails.qtyonhand;

                InventoryDetails.qtyonhand = difference;

                return StockOut(InventoryDetails);
            }

            return false;

        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL;
            var classname = "OctoPOSInventory, Method: Update";
            throw new OctoPOSIntegrationException(domain, classname, ex.Message);
        }

    }

    private static StringContent GetContent(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return new StringContent(json, Encoding.UTF8, "application/json");

    }

}
