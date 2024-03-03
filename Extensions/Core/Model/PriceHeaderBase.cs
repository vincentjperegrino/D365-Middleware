using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class PriceHeaderBase
    {
        [MaxLength(6)]
        public string evtNum { get; set; }
        [MaxLength(30)]
        public string evtDsc { get; set; }
        [MaxLength(8)]
        public string evtFdt { get; set; }
        [MaxLength(8)]
        public string evtTdt { get; set; }
    }
}
