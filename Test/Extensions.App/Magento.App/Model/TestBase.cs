using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMagento.App.Model;

public class TestBase
{

    public KTI.Moo.Extensions.Magento.Service.Config _config = new()
    {
        companyid = "3388",
        defaultURL = "https://novateurdev.argomall.ph/rest/default/V1",
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


    public KTI.Moo.Extensions.Magento.Service.Config Prodconfig = new()
    {
        companyid = "3389",
        defaultURL = "https://www.nespresso.ph//rest/default/V1",
        username = "jing-admin1",
        password = "Passw0rd",
        redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
    };


    public KTI.Moo.Extensions.Magento.Service.Config ProdToStagingconfig = new()
    {
        companyid = "3388",
        defaultURL = "https://nespresso.ph/rest/default/V1",
        username = "jing-admin1",
        password = "Passw0rd",
        redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
    };

    public string redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

    public string _connectionstringProd = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencci;AccountKey=ToNHaXe9NF8YkwGA3u7Ec5we8Ykf2fI6bRdBOml3Xmnv2AW8KwMNhQRkh0v4Cl7ccSkygk3JU6wt+AStFfpjkw==;EndpointSuffix=core.windows.net";

    public string _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
    public string _connectionstringKTI = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net";
    public string _connectionstringKTIDev = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragedev;AccountKey=JYPEw/njeKthpGVUe+Pbc4oNRXElEvvjPZQfevdb3KMe+qIUOvCbEPIOkipA4tGxxgavBd49pvND+AStnbnrkQ==;EndpointSuffix=core.windows.net";

}
