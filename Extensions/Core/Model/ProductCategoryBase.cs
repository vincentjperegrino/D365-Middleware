using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class ProductCategoryBase
    {
        public virtual string department { get; set; }
        public virtual string sub_dept { get; set; }
        public virtual string cy_class { get; set; }
        public virtual string sub_class { get; set; }
        public virtual string name { get; set; }
        public virtual string planned_gm { get; set; }
    }
}
