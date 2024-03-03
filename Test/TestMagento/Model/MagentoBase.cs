using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.Magento.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMagento.Model
{
    public class MagentoBase
    {
        public KTI.Moo.Extensions.Magento.Service.Config config = new()
        {
            companyid = "3388",
            defaultURL = "https://nespresso.novateurshop.com/rest/default/V1",
            username = "nova-admin",
            password = "passw0rd",
            redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
        };

        public KTI.Moo.Extensions.Magento.Service.Config Stagingconfig = new()
        {
            companyid = "3388",
            defaultURL = "https://nespresso.novateurshop.com/rest/default/V1",
            username = "jing-admin1",
            password = "Passw0rd",
            redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
        };


        public static string defaultURL = "https://nespresso.novateurshop.com/rest/default/V1";


        public string username = "nova-admin";
        public string password = "passw0rd";

        public string redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";
    }
}
