using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor.Interfaces
{
    public interface IChannelAppModuleProcessor
    {
        //ProcessorSuccessReturnModel ProcessAsync(string pollContent);
        ProcessorReturnModel ProcessAsync(string pollContent, ILogger log);
    }
}
