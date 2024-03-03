using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model.Queue;

public class Message
{
    [JsonProperty("seller_id")]
    public string SellerId { get; set; }
    [JsonProperty("message_type")]
    public MessageTypes Type { get; set; }
    [JsonProperty("timestamp")]
    public UInt64 Timestamp { get; set; }
    [JsonProperty("site")]
    public string Site { get; set; }

    [JsonProperty("data")]
    public Dictionary<string, dynamic> Data { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
