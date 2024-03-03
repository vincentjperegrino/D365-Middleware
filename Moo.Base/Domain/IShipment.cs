using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain;

public interface IShipment
{
    Task<string> upsert(string content, ILogger log);
    Task<string> update(string content ,string id, ILogger log);


}
