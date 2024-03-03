using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOctoPOS.Model
{
    public class OctoPOSBase
    {
        public static string defaultURL = "http://202.148.162.33:8080/NESPRESSOTEST/api.orm/1.0";

        public static string username = "NESPRESSOTEST";
        public static string password = "XHTMgtXRTZ";

        public static string ApiAuth = "MTAyMTk3MTgwNjE5MDQ1MTM3MDcwNTExMDE5NzQ5MDk2OTA1ODk3NQ==,MTIxNTAxNTMwOTE3MTA5Mzk5MDg5MTY0NTA2Njk2MDcyODMwMjY0NQ==";

        public static string redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

        public static string companyid = "3388";

        public KTI.Moo.Extensions.OctoPOS.Service.Config CongfigTest = new()
        {
           apiAuth = ApiAuth,
           redisConnectionString = redisConnectionString,
           companyid = companyid,
           username = username,
           password = password,
           defaultURL = defaultURL
      
        };

        ///Un comment when using only

        public KTI.Moo.Extensions.OctoPOS.Service.Config ConfigProduction = new()
        {
            apiAuth = "TU9PQ0VOVFJBTEFQSVVTRVI=,WkVkV2VtUkhVa2hXYm5CclVWRQ==",
            redisConnectionString = redisConnectionString,
            companyid = "3389",
            username = "MOOCENTRALUSER",
            password = "ZEdWemRHUkhWbnBrUVE",
            defaultURL = "http://129.126.131.84:8080/NESPRESSO/api.orm/1.0"

        };

        public KTI.Moo.Extensions.OctoPOS.Service.Config ProdToStagingconfig = new()
        {
            apiAuth = "TU9PQ0VOVFJBTEFQSVVTRVI=,WkVkV2VtUkhVa2hXYm5CclVWRQ==",
            redisConnectionString = redisConnectionString,
            companyid = "3388",
            username = "MOOCENTRALUSER",
            password = "ZEdWemRHUkhWbnBrUVE",
            defaultURL = "http://129.126.131.84:8080/NESPRESSO/api.orm/1.0"

        };


        public KTI.Moo.Extensions.OctoPOS.Service.Config NewProdToStagingconfig = new()
        {
            apiAuth = "TUFHRU5UT0FQSVVTRVI=,WlMxRThLdG9FYWxZZ3cx",
            redisConnectionString = redisConnectionString,
            companyid = "3388",
            username = "magento",
            password = "CFGDNyIUZC",
            defaultURL = "http://129.126.131.84:8080/NESPRESSO/api.orm/1.0"

        };
    }
}
