using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Helper.HashingAndEncryption
{
    public class HashingAndEncryption
    {
        public const int _saltLength = 16;
        public const int _hashSize = 16;
        public const int _iterationCount = 10000;

        public const int _IV = 16;
        public const string _key = "6v9y$B&E)H@McQfThWmZq4t7w!z%C*F-";

        public const string salesChannel_pepper = "U6nMZJ38Q2M=_rsf";
        public const string employee_pepper = "Av%24qTsXg^wXG=j";
    }
}
