using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;

namespace CRM_Plugin.Core.Domain
{
    public interface IEmployee
    {
        bool upsert(Entity _employee);
        bool create(Entity _employee);
        bool update(Entity _employee, Guid _id);
    }
}
