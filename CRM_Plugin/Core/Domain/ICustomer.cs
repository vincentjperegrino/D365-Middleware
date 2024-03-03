using CRM_Plugin.Core.Model;
using Microsoft.Xrm.Sdk;
using System;

namespace CRM_Plugin.Core.Domain
{
    public interface ICustomer
    {
        Entity Get(Guid ExistingID);
        Entity Get(string sourceid, OptionSetValue channelid);
        Entity GetByMobile(string Mobile);
        Entity GetByEmail(string Email);
        bool Upsert(Entity customer);
        Guid UpsertWithGuid(Entity customer);

        bool Create(Entity customer);
        Guid CreateWithGuid(Entity customer);

        bool Update(Entity customer, Guid ExistingID);

    }
}
