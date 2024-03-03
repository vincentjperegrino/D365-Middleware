using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Processor
{
    public interface IExtensionAppProcessorDomain
    {
        Task<bool> WritePollFileAsync(string fileName, string content);
        string ReadPollFile(string fileName);

    }
}
