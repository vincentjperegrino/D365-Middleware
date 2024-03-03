using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCyware.Model
{
    public class CywareBase
    {
        public KTI.Moo.Extensions.Cyware.Services.Config config = new()
        {
            RootFolder = "/files/",
            Host = "royalsftp.socialwifi.tv",
            Port = 22,
            Username = "testadmin",
            Password = "testadmin@123"
        };

    }
}
