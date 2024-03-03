using System.Text.Json;
using System.Text.Json.Serialization;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class ShipmentProvider
    {
        public string name { get; set; }
        public bool cod { get; set; }
        public bool is_default { get; set; }
        public bool api_integration { get; set; }
        public string tracking_code_example { get; set; }
        public string tracking_code_validation_regex { get; set; }
        public string enabled_delivery_options { get; set; }
        public string tracking_url { get; set; }

        public ShipmentProvider() { }

        public ShipmentProvider(string json)
        {
            var ship = JsonDocument.Parse(json).RootElement;
            if (ship.TryGetProperty("tracking_code_example", out var tracking))
                this.tracking_code_example = tracking.GetString();
            if (ship.TryGetProperty("enabled_delivery_options", out var deliveryOptions))
                this.enabled_delivery_options = deliveryOptions.GetString();
            if (ship.TryGetProperty("name", out var name))
                this.name = name.GetString();
            if (ship.TryGetProperty("cod", out var cod))
                this.cod = cod.ToString().Equals("1");
            if (ship.TryGetProperty("tracking_code_validation_regex", out var regex))
                this.tracking_code_validation_regex = regex.GetString();
            if (ship.TryGetProperty("is_default", out var def))
                this.is_default = def.ToString().Equals("1");
            if (ship.TryGetProperty("tracking_url", out var url))
                this.tracking_url = url.GetString();
            if (ship.TryGetProperty("api_integration", out var api))
                this.api_integration = api.ToString().Equals("1");
        }
    }
}