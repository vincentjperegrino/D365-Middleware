using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Function
{
    public class GetAllProducts
    {
        private readonly Domain.Product _product;

        public GetAllProducts(IOrganizationService service)
        {
            this._product = new Domain.Product(service);
        }

        public EntityCollection Process()
        {
            return _product.GetAll();            
        }
    }
}
