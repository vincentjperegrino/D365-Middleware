using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Service;

public class ConfigBase
{
    public virtual string defaultURL { get; init; }
    public virtual string redisConnectionString { get; init; }
    public virtual string companyid { get; init; }

}
