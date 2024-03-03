using System;
using System.Collections.Generic;
using System.Text.Json;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;

namespace KTI.Moo.Extensions.Lazada.Domain
{
    public class DataMoat
    {
        private Service.ILazopService _service { get; init; }

        public DataMoat(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public DataMoat(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public DataMoat(Service.ILazopService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Calls the Lazada Datamoat endpoint for recording successful/unsuccessful logins
        /// </summary>
        /// <param name="userId">The account which the Lazada seller uses to login to the app</param>
        /// <param name="tid">The account which the Lazada seller uses to login to Lazada</param>
        /// <param name="ip">The IPv4 address from which the login attempt was made</param>
        /// <param name="ati">The generated fingerprint of the device the login attempt was made from</param>
        /// <param name="loginResult">Whether the login was successful or failed. Use "success" or "fail"</param>
        /// <param name="loginMessage">Custom information such as reasons for failed login attempts. "invalid username/password," "account does not exist," etc.</param>
        /// <returns></returns>
        public bool RecordLogin(string userId, string tid, string ip, string ati, string loginResult, string loginMessage = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"time", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString()},
                {"appName", "Moo"},
                {"userId", userId},
                {"tid", tid},
                {"userIp", ip},
                {"ati", ati},
                {"loginResult", loginResult},
                {"loginMessage", loginResult.Equals("success") && string.IsNullOrWhiteSpace(loginMessage) ? "success" : loginMessage}
            };

            var response = this._service.ApiCall("/datamoat/login", parameters, "GET", false);
            return JsonDocument.Parse(response).RootElement.GetProperty("result").GetProperty("success").GetBoolean();
        }

        public Risk GetRisk(string userId, string ip, string ati)
        {
            var parameters = new Dictionary<string, string>
            {
                {"time", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString()},
                {"appName", "Moo"},
                {"userId", userId},
                {"userIp", ip},
                {"ati", ati}
            };

            var response = this._service.ApiCall("/datamoat/compute_risk", parameters, "GET", false);

            return new Risk(JsonDocument.Parse(response).RootElement.GetProperty("result").ToString());
        }
    }
}
