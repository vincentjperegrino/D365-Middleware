using System;
using System.Text.Json;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class Risk
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public float Value { get; set; }
        public bool IsRisk { get => this.Value > 0.5; }

        public Risk() { }

        public Risk(string json)
        {
            var riskJson = JsonDocument.Parse(json).RootElement;

            this.Type = riskJson.GetProperty("riskType").GetString();
            this.Description = riskJson.GetProperty("riskDescription").GetString();
            this.Value = float.Parse(riskJson.GetProperty("risk").ToString());
        }
    }
}