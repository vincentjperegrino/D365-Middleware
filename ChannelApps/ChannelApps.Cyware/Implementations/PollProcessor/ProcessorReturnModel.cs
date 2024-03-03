using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Cyware.Implementations.PollProcessor
{
    public class ProcessorReturnModel
    {
        public ProcessorSuccessReturnModel SuccessReturnModel { get; set; }
        public IEnumerable<ProcessorErrorReturnModel> ErrorReturnModel { get; set; }

        public ProcessorReturnModel(ProcessorSuccessReturnModel successReturnModel, IEnumerable<ProcessorErrorReturnModel> errorReturnModel)
        {
            SuccessReturnModel = successReturnModel;
            ErrorReturnModel = errorReturnModel;
        }
    }
}
