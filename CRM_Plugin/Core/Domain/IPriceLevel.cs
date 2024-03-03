using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Core.Domain
{
    public interface IPriceLevel<T> where T : PriceLevelBase
    {
        bool upsert(T _priceLevel);
        bool create(T _priceLevel);
        bool update(T _priceLevel, Entity _upPriceLevel);
    }
}
