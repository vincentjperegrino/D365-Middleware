using KTI.Moo.Extensions.Magento.Model.DTO.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Orders
{
    public class Search  : Core.Model.SearchBase<Model.Order>
    {

        [JsonProperty("search_criteria")]
        public SearchCriteria search_criteria { get; set; }

        [JsonProperty("items")]
        public override List<Model.Order> values { get; set; }

        [JsonProperty("total_count")]
        public override int total_count { get; set; }

        public override int page_size
        {
            get
            {
                if (search_criteria is not null)
                {
                    return search_criteria.page_size;
                }
                return default;
            }

        }

        public override int current_page
        {
            get
            {
                if (search_criteria is not null)
                {
                    return search_criteria.current_page;
                }
                return default;
            }
        }
    }
}
