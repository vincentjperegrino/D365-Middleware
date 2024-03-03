using Newtonsoft.Json;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class VideoContent
    {
        [JsonProperty("media_type")]
        public string media_type { get; set; }

        [JsonProperty("video_description")]
        public string description { get; set; }

        [JsonProperty("video_metadata")]
        public string metadata { get; set; }

        [JsonProperty("video_provider")]
        public string provider { get; set; }

        [JsonProperty("video_title")]
        public string title { get; set; }

        [JsonProperty("video_url")]
        public string url { get; set; } 

    }
}
