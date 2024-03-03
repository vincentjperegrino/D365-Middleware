using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Core.Domain
{
    public interface IProduct<T> where T : ProductBase
    {
        bool upsert(T _product);
        bool create(T _product);
        bool update(T _product, Entity _upProduct);

        EntityCollection GetAll();

    }
}
