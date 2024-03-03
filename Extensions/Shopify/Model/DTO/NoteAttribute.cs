using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class NoteAttribute : ShopifySharp.NoteAttribute { 
    

        public NoteAttribute() { }

        public NoteAttribute(Model.NoteAttribute noteAttribute) {


            Name = noteAttribute.name;
            Value = noteAttribute.value;


        }


    
    }
}
