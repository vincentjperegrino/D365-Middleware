using KTI.Moo.Extensions.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCyware.App
{
    public class Family : Model.TestBase
    {
        private readonly IBlobService blobService;
        private readonly KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog pollLog;
        public Family()
        {
            blobService = new KTI.Moo.Extensions.Cyware.Services.BlobService(config);
            pollLog = new KTI.Moo.Extensions.Cyware.App.Receiver.Receivers.PollLog(blobService);
        }
    }
}
