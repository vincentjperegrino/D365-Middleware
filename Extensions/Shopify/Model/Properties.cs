using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Properties
{
    public Properties() { }

    public Properties(ShopifySharp.LineItemProperty properties) {

        name = properties.Name;
        value = properties.Value;
    
    }

    public object name { get; set; }   
    public object value { get; set; }


}