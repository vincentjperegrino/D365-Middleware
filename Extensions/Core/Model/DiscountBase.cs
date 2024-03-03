using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class DiscountBase
    {
        public string evtNum { get; set; }
        public string evtDsc { get; set; }
        public string evtFdt { get; set; }
        public string evtTdt { get; set; }
    }
}
