using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Service.Queue
{
    public class Config : ConfigBase
    {
        public string AppKey { get; init; }
        public string AppSecret { get; init; }
        public int MaxRetries { get; init; }
        public int MaxPaginationInFinanceAPI { get; init; }

        public string BaseSourceUrl { get; init; }

        public string Region { get; init; }
        public string SellerId { get; init; }
    }
}
