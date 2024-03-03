using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class TelephoneBase : CreationDateBase
    {

        [StringLength(50)]
        public virtual string telephone { get; set; }

        public virtual bool primary { get; set; }



    }
}
