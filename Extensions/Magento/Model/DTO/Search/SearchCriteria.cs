using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Search
{
    public class SearchCriteria
    {
        public List<FilterGroups> filter_groups { get; set; }

        public int page_size { get; set; }
        public int current_page { get; set; }

    }
}
