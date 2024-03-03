using KTI.Moo.Extensions.Core.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace KTI.Moo.Extensions.OctoPOS.Service
{
    public class Config : ConfigBase
    {
        public string username { get; init; }
        public string password { get; init; }
        public string apiAuth { get; init; }

        //new public string defaultURL { get; init; }
        //new public string redisConnectionString { get; init; }
        //new public string companyid { get; init; }

        public Config()
        {

        }

    }
}
