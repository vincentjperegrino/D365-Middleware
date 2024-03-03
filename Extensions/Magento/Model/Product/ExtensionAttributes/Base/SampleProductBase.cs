using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model.Base
{
    public class SampleProductBase
    {
        public string sample_file { get; set; }

        public DownloadableProductLinksFileContent sample_file_content { get; set; }

        public string sample_type { get; set; }

        public string sample_url { get; set; }

        public int sort_order { get; set; }

        public string title { get; set; }
    }
}
