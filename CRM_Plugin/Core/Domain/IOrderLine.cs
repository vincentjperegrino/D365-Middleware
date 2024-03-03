using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Domain
{
    public interface IOrderLine
    {
        Entity Get(Guid ExistingID);
        Entity Get(Guid ExistingSalesOrderID, string lineNumber);
        EntityCollection GetOrderItemList(Guid ExistingSalesOrderID);
        bool Upsert(Guid SalesOrderID, Entity orderLine);
        Guid UpsertWithGuid(Guid SalesOrderID, Entity orderLine);

        bool Create(Guid SalesOrderID, Entity orderLine);
        Guid CreateWithGuid(Guid SalesOrderID, Entity orderLine);

        bool Update(Guid SalesOrderID, Entity orderLine, Guid ExistingID);
    }
}
