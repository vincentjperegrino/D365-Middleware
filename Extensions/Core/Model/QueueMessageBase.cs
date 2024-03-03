using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class QueueMessageBase
    {
        public string Action { get; set; }
        public string Body { get; set; }
        public string QueueDateTime { get; set; }
    }
}
