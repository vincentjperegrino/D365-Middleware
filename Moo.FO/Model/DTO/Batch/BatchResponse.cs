using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model.DTO.Batch
{
    public class BatchResponse
    {
        public List<string> StatusCode { get; set; }
        public List<string> Location { get; set; }
    }
}
