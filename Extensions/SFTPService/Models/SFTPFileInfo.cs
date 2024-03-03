using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.SFTPService.Models
{
    public class SFTPFileInfo
    {
        public class SFTPConnectionInfo
        {
            public string host { get; set; }
            public int port { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
