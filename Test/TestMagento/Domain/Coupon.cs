using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;


namespace TestMagento.Domain;

public class Coupon : MagentoBase
{

    [Fact]
    public void AddCouponRuleWorking()
    {

        KTI.Moo.Extensions.Magento.Domain.Coupon MagentoCouponDomain = new(defaultURL, redisConnectionString, username, password);

        KTI.Moo.Extensions.Magento.Model.PromoCoupon CouponRuleModel = new();

        CouponRuleModel.kti_promocode = "50% off";
        CouponRuleModel.kti_discountamount = (decimal)50.5;

        var response = MagentoCouponDomain.Add(CouponRuleModel);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.PromoCoupon>(response);

    }

    [Fact]
    public void AddCouponCustomerWorking()
    {

        KTI.Moo.Extensions.Magento.Domain.Coupon MagentoCouponDomain = new(defaultURL, redisConnectionString, username, password);

        KTI.Moo.Extensions.Magento.Model.PromoCouponList CouponCustomerModel = new();

        CouponCustomerModel.rule_id = 20;
        CouponCustomerModel.customer_id = 67;
        CouponCustomerModel.kti_uniquepromocode = "KTIsampleCode1";


        var response = MagentoCouponDomain.AddCoupon(CouponCustomerModel);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.PromoCouponList>(response);

    }

}
