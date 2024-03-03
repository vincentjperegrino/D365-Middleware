using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Core.Domain
{
    public interface ICurrency<T> where T : CurrencyBase
    {
        bool upsert(T _currency);
        bool create(T _currency);
        bool update(T _currency, Entity _upCurrency);
    }
}
