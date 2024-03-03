using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Lazada.Domain
{
    public class ShipmentProvider
    {
        private Service.ILazopService _service { get; init; }

        public ShipmentProvider(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public ShipmentProvider(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public ShipmentProvider(Service.ILazopService service)
        {
            this._service = service;
        }
        public ShipmentProvider(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
        }

        public ShipmentProvider(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);
        }
        public IEnumerable<Model.ShipmentProvider> Get()
        {
            var response = this._service.AuthenticatedApiCall("/shipment/providers/get", null, "GET");
            var json = JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("shipment_providers");

            return json.EnumerateArray().Select(s => new Model.ShipmentProvider(s.ToString()));
        }
    }
}
