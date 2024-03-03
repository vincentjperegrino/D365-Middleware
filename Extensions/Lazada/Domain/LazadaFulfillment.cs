using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments;
using KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments.Packages;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Domain;

public class Fulfillment : Core.Domain.IFulfillment<Model.PackageOrder, Model.OrderItempackage>
{

    private Service.ILazopService _service { get; init; }

    private readonly string _sourceURL = "https://sellercenter.lazada.com.ph/order/detail";

    public Fulfillment(Service.Queue.Config config, IDistributedCache cache)
    {
        this._service = new LazopService(config, cache);

        this._sourceURL = config.BaseSourceUrl;
    }   
    
    
    public Fulfillment(Service.Queue.Config config, ClientTokens clientTokens)
    {
        this._service = new LazopService(config, clientTokens);

        this._sourceURL = config.BaseSourceUrl;
    }

    public bool DeliverDigital(string orderid, List<string> orderitemids)
    {
        throw new NotImplementedException();
    }

    public string GetShipmentProvider(string orderid, List<string> orderitemids)
    {
        if (orderid == null || orderitemids == null)
        {
            throw new System.ArgumentNullException("order and orderitems can't be null");
        }

        int maxbatchsize = 20;

        if (orderitemids.Count > maxbatchsize)
        {
            throw new System.Exception("Batch size upto 20 only");
        }


        var shipmentProviderDTO = new Model.DTO.Fulfillments.ShipmentProvider()
        {
            orders = new()
            {
                new()
                {
                   order_id = long.Parse(orderid),
                   order_item_ids = orderitemids.Select(items => long.Parse(items)).ToArray()
                }
            }
        };


        var parameters = new Dictionary<string, string>
        {
            {"getShipmentProvidersReq", JsonConvert.SerializeObject(shipmentProviderDTO)},
        };


        var APIresult = this._service.AuthenticatedApiCall("/order/shipment/providers/get", parameters, "GET");

        var ApiResultModel = JsonConvert.DeserializeObject<dynamic>(APIresult);

        return ApiResultModel.result.data.shipping_allocate_type;

    }

    public PackageOrder Pack(string orderid, List<string> orderitemids, string shipping_allocate_type)
    {
        return Pack(orderid, orderitemids, shipping_allocate_type, "dropship", null);
    }

    public PackageOrder Pack(string orderid, List<string> orderitemids, string shipping_allocate_type, string delivery_type)
    {
        return Pack(orderid, orderitemids, shipping_allocate_type, delivery_type, null);
    }

    public PackageOrder Pack(string orderid, List<string> orderitemids, string shipping_allocate_type, string delivery_type, string shipment_provider_code)
    {
        if (orderid == null || orderitemids == null)
        {
            throw new System.ArgumentNullException("order and orderitems can't be null");
        }

        int maxbatchsize = 20;

        if (orderitemids.Count > maxbatchsize)
        {
            throw new System.Exception("Batch size upto 20 only");
        }


        var PackRequirement = new Model.DTO.Fulfillments.PackRequirement()
        {
            pack_order_list = new()
            {
                new()
                {
                   order_id = long.Parse(orderid),
                   order_item_list = orderitemids.Select(items => long.Parse(items)).ToArray()
                }
            },
            shipping_allocate_type = shipping_allocate_type,
            delivery_type = delivery_type,
            shipment_provider_code = shipment_provider_code
        };

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore

        };

        var parameters = new Dictionary<string, string>
        {
            {"packReq", JsonConvert.SerializeObject(PackRequirement ,settings)},
        };

        var response = this._service.AuthenticatedApiCall("/order/fulfill/pack", parameters, "POST");

        var ApiResultModel = JsonConvert.DeserializeObject<Model.DTO.Fulfillments.Packages.PackageDTO>(response);

        return ApiResultModel.result.data.pack_order_list.FirstOrDefault();
    }


    public string PrintAWB(string packageid)
    {
        var AWBRequirement = new Model.DTO.Fulfillments.AWBDocumentReq()
        {
            packages = new()
           {
               new()
               {
                   package_id = packageid
               }
           }
        };

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var parameters = new Dictionary<string, string>
        {
            {"getDocumentReq", JsonConvert.SerializeObject(AWBRequirement ,settings)},
        };


        var APIresult = this._service.AuthenticatedApiCall("/order/package/document/get", parameters, "GET");

        var ApiResultModel = JsonConvert.DeserializeObject<dynamic>(APIresult);

        return ApiResultModel.result.data.pdf_url;
    }

    public bool ReadyToShip(string packageid)
    {
        var ReadyToShipRequirement = new Model.DTO.Fulfillments.ReadyToShip()
        {
            packages = new()
           {
               new()
               {
                   package_id = packageid
               }
           }
        };

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var parameters = new Dictionary<string, string>
        {
            {"readyToShipReq", JsonConvert.SerializeObject(ReadyToShipRequirement ,settings)},
        };


        var APIresult = this._service.AuthenticatedApiCall("/order/package/rts", parameters, "POST");

        var ApiResultModel = JsonConvert.DeserializeObject<Model.DTO.Fulfillments.ReadyToShips.ReadyToShipDTO>(APIresult);

        foreach (var result in ApiResultModel.result.data.packages)
        {

            if (result.item_err_code != "0")
            {
                throw new System.Exception($"Error code {result.item_err_code}, {result.msg}");
            }

        }

        return true;
    }

    public OrderItempackage RecreatePackage(string packageid)
    {
        var RePackRequirement = new Model.DTO.Fulfillments.RePackRequirement()
        {
           packages = new()
           {
               new()
               {
                   package_id = packageid
               }
           }
        };

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var parameters = new Dictionary<string, string>
        {
            {"rePackReq", JsonConvert.SerializeObject(RePackRequirement ,settings)},
        };


        var APIresult = this._service.AuthenticatedApiCall("/order/package/repack", parameters, "POST");

        var ApiResultModel = JsonConvert.DeserializeObject<Model.DTO.Fulfillments.RePackages.RePackageDTO>(APIresult);

        return ApiResultModel.result.data.packages.FirstOrDefault();    
    }

}
