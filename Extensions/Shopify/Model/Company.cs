using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Company
{

    public Company() { }    
    public Company(ShopifySharp.OrderCompany company)
    {

        id = company.Id ?? default;
        location_id = company.LocationId ?? default;

    }

    public long id { get; set; }
    public long location_id { get; set; }




}
