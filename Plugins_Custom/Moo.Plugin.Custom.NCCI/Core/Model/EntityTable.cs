using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Core.Model
{
    public class EntityTable
    {
        public static readonly string EntityName = "EntityName";
        public static readonly ColumnSet ColumnSet = new ColumnSet(true);
    }
}
