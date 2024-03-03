using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class PriceSet
{

    public PriceSet() { }

    public PriceSet(ShopifySharp.PriceSet priceSet) {


        if (priceSet.ShopMoney != null)
        {
            shop_money = new Model.ShopMoney(priceSet.ShopMoney);

        }

        if (priceSet.PresentmentMoney != null)
        {
            presentment_money = new Model.PresentmentMoney(priceSet.PresentmentMoney);

        }


    }


    public ShopMoney shop_money { get; set; }
    public PresentmentMoney presentment_money { get; set; }


}