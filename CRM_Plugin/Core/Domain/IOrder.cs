using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Domain
{
    public interface IOrder
    {
        Entity Get(Guid ExistingID);
        Entity Get(string sourceid, OptionSetValue channel);
        bool Upsert(Entity order);
        Guid UpsertWithGuid(Entity order);
        bool Create(Entity order);
        Guid CreateWithGuid(Entity order);
        bool Update(Entity order, Guid ExistingID);
    }
}
