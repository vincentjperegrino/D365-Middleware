

namespace TestSAP.Model;

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


}
