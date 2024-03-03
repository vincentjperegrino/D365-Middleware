using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware
{
    public class QueueProcessorModel
    {
        public int Count { get; set; }
        public string FileName { get; set; }
        public string ModuleType { get; set; }
        public int CompanyId { get; set; }
        public string BatchID { get; set; }
        public int BatchNumberr { get; set; } 
        public int TotalBatchCount { get; set; }
    }
}
