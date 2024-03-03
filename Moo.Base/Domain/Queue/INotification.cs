using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain.Queue;

public interface INotification
{
    bool Notify(string WebhookUrl, string Title, string Message, ILogger log);
}
