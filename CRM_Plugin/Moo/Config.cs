using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Moo
{
    public class Config
    {

        #region Properties
        public static string baseurl = "https://kationtech-moo.azurewebsites.net";
    //   public static string baseurl = "https://localhost:44346";


        public static string authurl = "https://kationtechservices.azure-api.net";
        public static string auth_path = "/auth/token";
        public static string replicatecustomer_path = "/api/customer/replicate";
        public static string replicatepromo_path = "/api/promo/replicate";
        public static string replicatepromocoupon_path = "/api/promo/promocoupon/replicate";
        public static string replicateunitgroup_path = "/api/unitgroup/replicate";
        public static string replicateunitofmeasurement_path = "/api/unitofmeasurement/replicate";
        public static string replicatepricelist_path = "/api/pricelist/replicate";
        public static string replicateproductpricelevel_path = "/api/productpricelevel/replicate";
        public static string replicateproduct_path = "/api/product/replicate";
        public static string company_33387_occapikey = "75e9bf5d-95d8-4011-8898-a19f2a55f8e1";
        public static string company_33387_subkey = "d6a39667e32041228cf93b1dc93cd5a3";
        public static string company_33388_occapikey = "d4bef8ea-08cd-47b2-9788-29a9c4da3cbd";
        public static string company_33388_subkey = "d6a39667e32041228cf93b1dc93cd5a3";
        public static string promotexter_base_url = "https://rest-portal.promotexter.com/";
        public static string promotexter_sendsms_path = "sms/send";
        public static string promotexter_sendviber_path = "viber/send";



        //public static string company_3387_occapikey = "da8c5f39-138c-4df6-b750-6b5c45d5ccc4";
        //public static string company_3387_subkey = "d6a39667e32041228cf93b1dc93cd5a3";
        public static string rediscacheStoreConfig = "/api/ChannelManagement/StoreConfig/SetCache";

        public static string replicateorder_path = "/api/sales/order/replicate";
        public static string replicateorderstatus_path = "/api/sales/order/replicate/status";
        public static string replicateinvoice_path = "/api/invoice/replicate";


        public static string DEV_PRICELIST_DEFAULT = "regular";
        public static string DEV_PRICELIST_SALE = "sale";

        public static string NCCI_PRICELIST_DEFAULT = "standard";
        public static string NCCI_PRICELIST_SALE = "sale";


        public static string RedisCacheConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

        public static int CompanyID_KTI_DEV = 3387;
        public static int CompanyID_NCCI_TEST = 3388;
        public static int CompanyID_NCCI_PROD = 3389;



        #endregion

        public static Dictionary<string, string> GetOriginCredentials()
        {
            Dictionary<string, string> originCredentials = new Dictionary<string, string>();

            originCredentials.Add("3387_occapikey", company_33387_occapikey);
            originCredentials.Add("3387_subkey", company_33388_subkey);
            originCredentials.Add("3388_occapikey", company_33388_occapikey);
            originCredentials.Add("3388_subkey", company_33388_subkey);

            return originCredentials;
        }
    }
}
