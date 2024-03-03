using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Validation
{
    internal class Validation
    {

        public bool valid { get; set; } 

        public string[] messages { get; set; }

        public string message { get; set; }
    }
}
