using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class CreationDateBase
    {
        public virtual DateTime created_date { get; set; }

        public virtual DateTime updated_date { get; set; }
    }
}
