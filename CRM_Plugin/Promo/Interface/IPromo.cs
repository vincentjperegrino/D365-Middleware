using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Promo.Interface
{
    interface IPromo
    {
        bool Process(Entity _entity);
    }
}
