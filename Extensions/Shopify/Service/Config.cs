using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Service;

public class Config : ConfigBase
{

    public string baseurl { get; set; }
    public string version { get; init; }
    public string admintoken { get; init; }

    public override string defaultURL { get => baseurl + "/" + version + "/"; }

}
