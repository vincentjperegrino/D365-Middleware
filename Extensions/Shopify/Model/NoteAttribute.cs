using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class NoteAttribute { 

    public NoteAttribute() { }

    public NoteAttribute(ShopifySharp.NoteAttribute noteAttribute) {

        name = noteAttribute.Name;
        value = noteAttribute.Value;

    
    
    }

    public string name { get; set; }
    public object value { get; set; }
}