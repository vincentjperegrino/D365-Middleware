using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain;

public interface IPromo<GenericPromoCoupon, GenericPromoCouponList> where GenericPromoCoupon : Model.PromoBase where GenericPromoCouponList : Model.PromoCouponBase
{
    GenericPromoCoupon Get(string promoCode);

    GenericPromoCoupon Add(GenericPromoCoupon promoDetails);

    GenericPromoCouponList GetCoupon(string couponCode);

    GenericPromoCouponList AddCoupon(GenericPromoCouponList couponDetails);
}
