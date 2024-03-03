using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.App.Receiver.Models
{
    public class BasePollLog
    {
        public string StoreNumber { get; set; }
        public string TransDate { get; set; }
        public string RegisterID { get; set; }
        public string RollOverCode { get; set; }
        public string TransNumber { get; set; }
        public string TransSeq { get; set; }
        public string SubSeq { get; set; }
        public string StringType { get; set; }
    }
}
