


namespace Operation.SAP.Test;

public class SAPBase
{
    public KTI.Moo.Extensions.SAP.Service.Config config = new()
    {
        companyid = "3388",
        defaultURL = "https://fgh.nespresso.ph.novateur.ph:30030/b1s/v1",
        username = "manager",
        password = "n0v@rod",
        companyDB = "SBOTEST_FINANCE",
        redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
    };

    public KTI.Moo.Extensions.SAP.Service.Config configProd = new()
    {
        companyid = "3389",
        defaultURL = "https://fgh.nespresso.ph.novateur.ph:30030/b1s/v1",
        username = "manager",
        password = "n0v@rod",
        companyDB = "SBOLIVE_NOVATEUR3",
        redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
    };

    public static string redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

    public string _connectionstringProd = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencci;AccountKey=ToNHaXe9NF8YkwGA3u7Ec5we8Ykf2fI6bRdBOml3Xmnv2AW8KwMNhQRkh0v4Cl7ccSkygk3JU6wt+AStFfpjkw==;EndpointSuffix=core.windows.net";

    public string _connectionstring = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";
    public string _connectionstringKTI = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net";
    public string _connectionstringKTIDev = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragedev;AccountKey=JYPEw/njeKthpGVUe+Pbc4oNRXElEvvjPZQfevdb3KMe+qIUOvCbEPIOkipA4tGxxgavBd49pvND+AStnbnrkQ==;EndpointSuffix=core.windows.net";
}
