using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor
{
    public class ProcessorSuccessReturnModel
    {
        public string ConcatenatedResult { get; set; }
        public int Count { get; set; }

        public ProcessorSuccessReturnModel(string concatenatedResult, int count)
        {
            ConcatenatedResult = concatenatedResult;
            Count = count;
        }
    }
}
