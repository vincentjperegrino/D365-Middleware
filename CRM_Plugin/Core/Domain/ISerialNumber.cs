using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Domain
{
    public interface ISerialNumber
    {
        bool upsert(Entity orderLine);
        bool create(Entity orderLine);
        bool update(Entity orderLine, Guid ExistingID);
    }
}
