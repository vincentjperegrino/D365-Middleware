using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KTI.Moo.Extensions.Lazada.Model;

public class Address : Core.Model.AddressBase
{

    public bool default_billing { get; set; }
    public bool default_shipping { get; set; }

    public List<Telephone> telephone { get; set; }

    public Address()
    {
        telephone = new List<Telephone>();
    }

}




