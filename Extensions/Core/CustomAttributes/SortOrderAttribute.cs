using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.CustomAttributes
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SortOrderAttribute : Attribute
    {
        public int Order { get; }

        public SortOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
