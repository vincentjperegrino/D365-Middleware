using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain.Queue;

public interface IOrderStatusMessage
{
    public bool Process(string message , ILogger log);
    public bool CheckOrderStatus(string orderid, string orderstatus, ILogger log);
}
