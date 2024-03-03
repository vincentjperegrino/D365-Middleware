using System;
using System.Text.Json.Serialization;

namespace KTI.Moo.Extensions.Lazada.Model
{
    public class ClientTokens : Core.Model.ClientTokensBase
    {
        [JsonPropertyName("access_token")]
        public override string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public override string RefreshToken { get; set; }
        public override DateTime AccessExpiration { get; set; }
        public override DateTime? RefreshExpiration { get; set; } = null;

        [JsonPropertyName("account_id")]
        public override string Id { get; set; }
        public string Account { get; set; }

        public string Country { get; set; }
        [JsonPropertyName("account_platform")]
        public string AccountPlatform { get; set; }
        [JsonPropertyName("country_user_info")]
        public CountryUserInfo[] CountryUserInfos { get; set; }
    }

    public record CountryUserInfo
    {
        public string Country { get; set; }

        [JsonPropertyName("short_code")]
        public string ShortCode { get; set; }

        [JsonPropertyName("user_id")]
        public string UserID { get; set; }

        [JsonPropertyName("seller_id")]
        public string SellerID { get; set; }
    }
}
