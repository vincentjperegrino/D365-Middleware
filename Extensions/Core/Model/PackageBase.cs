using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class PackageBase
{

    public virtual long order_id { get; set; }
    public virtual string kti_sourceid { get; set; }

}
