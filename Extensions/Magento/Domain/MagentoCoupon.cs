using Azure;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Domain;

public class Coupon : Core.Domain.IPromo<PromoCoupon, PromoCouponList>
{

    private readonly IMagentoService _service;
    public const string APIDirectory = "/lof-couponcode";
    private readonly string _companyid;

    public Coupon(Config config)
    {
        this._service = new MagentoService(config);
        _companyid = config.companyid;
    }

    public Coupon(Config config, IDistributedCache cache)
    {
        this._service = new MagentoService(config, cache);
        _companyid = config.companyid;
    }

    public Coupon(string defaultDomain, string redisConnectionString, string username, string password)
    {
        this._service = new MagentoService(defaultDomain, redisConnectionString, username, password);
    }

    public Coupon(IMagentoService service)
    {
        this._service = service;
    }

    /// <summary>
    /// returns the rule_id only
    /// </summary>
    /// <param name="ruleDetails"></param>
    /// <returns></returns>
    /// <exception cref="MagentoIntegrationException"></exception>
    public PromoCoupon Add(PromoCoupon ruleDetails)
    {
        if (ruleDetails is null)
        {
            throw new ArgumentNullException(nameof(ruleDetails));
        }

        try
        {
            var path = APIDirectory + "/rule";

            var method = "POST";

            var isAuthenticated = true;

            Model.DTO.Coupon.AddCouponRule couponRule = new();

            couponRule.rule = ruleDetails;


            var stringContent = GetContent(couponRule);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCouponData = JsonConvert.DeserializeObject<List<Model.DTO.Coupon.ResponseCouponRule>>(response, settings);

            if (ReturnCouponData is null || ReturnCouponData.Count() <= 0 || !ReturnCouponData.FirstOrDefault().success || string.IsNullOrWhiteSpace(ReturnCouponData.FirstOrDefault().rule_id))
            {
                throw new System.Exception(response);
            }

            ruleDetails.kti_channelid = ReturnCouponData.FirstOrDefault().rule_id;

            return ruleDetails;

        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + APIDirectory + "/rule";

            var classname = "MagentoCoupon, Method: AddRule";

            throw new MagentoIntegrationException(domain, classname, ex.Message);
        }

    }

    public PromoCouponList AddCoupon(PromoCouponList CouponCustomerDetails)
    {
        if (CouponCustomerDetails is null)
        {
            throw new ArgumentNullException(nameof(CouponCustomerDetails));
        }

        try
        {
            var path = APIDirectory + "/customer-coupon";

            var method = "POST";

            var isAuthenticated = true;

            Model.DTO.Coupon.AddCouponCustomer couponCustomer = new();

            couponCustomer.customer_coupon = CouponCustomerDetails;


            var stringContent = GetContent(couponCustomer);

            var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var ReturnCouponData = JsonConvert.DeserializeObject<List<Model.DTO.Coupon.ResponseCouponCustomer>>(response, settings);

            if (ReturnCouponData is null || ReturnCouponData.Count <= 0 || !ReturnCouponData.FirstOrDefault().success)
            {
                throw new System.Exception(response);
            }

            return CouponCustomerDetails;

        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + APIDirectory + "/customer-coupon";

            var classname = "MagentoCoupon, Method: AddCouponCustomer";

            throw new MagentoIntegrationException(domain, classname, ex.Message);

        }


    }


    public PromoCoupon Get(string promoCode)
    {
        if (string.IsNullOrWhiteSpace(promoCode))
        {
            throw new ArgumentException("Invalid promoCode", nameof(promoCode));
        }

        try
        {
            var path = APIDirectory + "/rule";

            var method = "GET";

            var isAuthenticated = true;

            var response = _service.ApiCall(path, method, isAuthenticated);

            var ReturnCouponData = JsonConvert.DeserializeObject<PromoCoupon>(response);

            return ReturnCouponData;
        }
        catch
        {
            return new PromoCoupon();
        }
    }


    public PromoCouponList GetCoupon(string couponCode)
    {
        throw new NotImplementedException();
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
