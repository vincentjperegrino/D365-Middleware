using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Company : ShopifySharp.OrderCompany
    
    {



        public Company() { 
        
        
        }


        public Company(Model.Company company) {


            Id = company.id;
            LocationId = company.location_id;





        }


    }
}
