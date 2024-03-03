using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class QueueErrorMessageBase
    {
        public string Data { get; set; }
        public string ErrorMessage { get; set; }
        public string? BatchResponse { get; set; }
    }
}
