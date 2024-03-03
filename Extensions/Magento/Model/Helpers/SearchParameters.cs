using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.Helpers
{
    public class SearchParameters
    {
        public int filter_groups { get; set; }
        public int filters{ get; set; }
        public string field { get; set; }
        public string value { get; set; }   
        public string condition_type { get; set; }  

    }
}
