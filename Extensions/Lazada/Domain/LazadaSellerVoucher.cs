using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Model.DTO.Fulfillments;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Domain;

public class SellerVoucher : IPromoPlatform<Lazada.Model.Promo, Lazada.Model.PromoProduct>
{

    private Service.ILazopService _service { get; init; }

    private readonly Service.Queue.Config _config;

    public SellerVoucher(string region, string sellerId)
    {
        this._service = new LazopService(
            Service.Config.Instance.AppKey,
            Service.Config.Instance.AppSecret,
            new Model.SellerRegion() { Region = region, SellerId = sellerId }
        );
    }
    public SellerVoucher(Service.Queue.Config config, IDistributedCache cache)
    {
        this._service = new LazopService(config, cache);
        _config = config;
    }
    public SellerVoucher(Service.Queue.Config config, ClientTokens clientTokens)
    {
        this._service = new LazopService(config, clientTokens);
        _config = config;
    }


    public SellerVoucher(string key, string secret, string region, string accessToken)
    {
        this._service = new LazopService(key, secret, region, accessToken, null);
    }
    public SellerVoucher(Service.ILazopService service)
    {
        this._service = service;
    }


    public bool Activate(string promocode)
    {
        throw new NotImplementedException();
    }

    public bool ActivateFromID(string promocodeid)
    {
        throw new NotImplementedException();
    }

    public Promo Add(Promo promo)
    {
        throw new NotImplementedException();
    }

    public bool AddProducts(List<PromoProduct> promo)
    {
        throw new NotImplementedException();
    }

    public bool Deactivate(string promocode)
    {
        throw new NotImplementedException();
    }

    public bool DeactivateFromID(string promocodeid)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProducts(List<PromoProduct> promo)
    {
        throw new NotImplementedException();
    }

    public Promo Get(string promocode)
    {
        throw new NotImplementedException();
    }

    public Promo GetFromID(string promocodeid)
    {
        return GetFromID(promocodeid, "COLLECTIBLE_VOUCHER");
    }

    public Promo GetFromID(string promocodeid, string voucherType)
    {
        try
        {

            var parameters = new Dictionary<string, string>
        {
            {"voucher_type",  voucherType },
            {"id", promocodeid}

        };

            var response = this._service.AuthenticatedApiCall("/promotion/voucher/get", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data").ToString();

            var ApiResultModel = JsonConvert.DeserializeObject<Model.Promo>(json);

            return ApiResultModel;

        }
        catch
        {

            return new Promo();
        }

    }

    public List<PromoProduct> GetProductList(string promocode)
    {
        throw new NotImplementedException();
    }

    public List<PromoProduct> GetProductListFromID(string promocodeid)
    {
        throw new NotImplementedException();
    }

    public Promo Update(Promo promo)
    {
        throw new NotImplementedException();
    }

    public Promo Upsert(Promo promo)
    {
        throw new NotImplementedException();
    }
}
