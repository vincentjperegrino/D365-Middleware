using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Core.Domain
{
    public interface IProductPriceLevel<T> where T : ProductPriceLevelBase
    {
        bool upsert(T _productPriceLevel);
        bool create(T _productPriceLevel);
        bool update(T _productPriceLevel, Entity _upProductPriceLevel);

    }
}
