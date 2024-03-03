using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLazada.Base
{
    public class LazadaBase
    {
        ////Get AccessToken in lazada
        ////https://open.lazada.com/app/index.htm?spm=a2o9m.11193535.0.0.17f338e4Zf6RXV#/api/test?apiPath=%2Ffinance%2Fpayout%2Fstatus%2Fget&appkey=100132&_k=1mstoz


        //authorize
        //https://auth.lazada.com/oauth/authorize?response_type=code&force_auth=true&redirect_uri=https://extensionslazadaappqueueprod.azurewebsites.net/api/callback/lazada&client_id=105046


        //public static string AccessToken = "50000600e00k12b61782jmxnriA3HXejpdUoEAcBsdNythkfSLvFqx9H4gAXlqUz";

        ////End. Update AccessToken every testing

        //public static string Region = "ph";
        //public static string SellerID = "500203125266";

        public static string _Region = "ph";
        public static string _AppKey = "105046";
        public static string _AppSecret = "sGFdQUXMsPZ9PSU3fPk8s8pQbXoAPM5N";
        public static string _DefaultUrl = "https://api.lazada.com/rest";

        public static string _SellerID = "500203125266";
        public static string AccessToken = "50000600d059H8pwo1e1cb894yBdaJvehtCrQGXCdPgrryhhmygXdvsokUUXz7zs";
        public static string redisconnection = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

        public KTI.Moo.Extensions.Lazada.Service.Queue.Config CongfigTest = new()
        {
            AppKey = _AppKey,
            AppSecret = _AppSecret,
            redisConnectionString= redisconnection,
            Region = _Region,
            SellerId = _SellerID,
            defaultURL = _DefaultUrl

        };     
        
        public KTI.Moo.Extensions.Lazada.Service.Queue.Config CongfigTNCCI = new()
        {
            AppKey = _AppKey,
            AppSecret = _AppSecret,
            redisConnectionString= redisconnection,
            Region = _Region,
            SellerId = "500160021491",
            defaultURL = _DefaultUrl

        };

    }
}
