using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Extensions.Core.CustomAttributes;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Cyware.Model.DTO
{
    public class POLL64DTO
    {
        [SortOrder(1)]
        [MaxLength(20)]
        public string evtNum { get; set; }
        [SortOrder(2)]
        [MaxLength(60)]
        public string evtDsc { get; set; }
        [SortOrder(3)]
        [MaxLength(8)]
        public string evtDft { get; set; }
        [SortOrder(4)]
        [MaxLength(8)]
        public string evtTdt { get; set; }

        PollMapping helper = new PollMapping();

        public POLL64DTO(string evtNum, string evtDsc, DateTime? evtDft, DateTime? evtTdt)
        {
            //var helper = new PollMappingHelper();
            this.evtNum = helper.FormatStringAddSpacePadding(evtNum, (typeof(POLL64DTO).GetProperty("evtNum").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.evtDsc = helper.FormatStringAddSpacePadding(evtDsc, (typeof(POLL64DTO).GetProperty("evtDsc").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.evtDft = helper.FormatDateToyyyyMMdd(evtDft);
            this.evtTdt = helper.FormatDateToyyyyMMdd(evtTdt);
        }
        public string Concat (POLL64DTO obj)
        {
            //var helper = new PollMappingHelper();
            return helper.ConcatenateValues(obj);
        }
    }
}
