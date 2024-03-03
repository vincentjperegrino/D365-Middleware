﻿using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Model.DTO.Search;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Customers
{
    public class Search : Core.Model.SearchBase<Model.Customer>
    {

        [JsonProperty("search_criteria")]
        public SearchCriteria search_criteria { get; set; }

        [JsonProperty("items")]
        public override List<Model.Customer> values { get; set; }

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
