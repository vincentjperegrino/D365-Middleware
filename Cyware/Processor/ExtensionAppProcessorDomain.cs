using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Processor
{
    public class ExtensionAppProcessorDomain : IExtensionAppProcessorDomain
    {
        private ISFTPService _sftpService { get; init; }

        public ExtensionAppProcessorDomain(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public async Task<bool> WritePollFileAsync(string fileName, string content)
        {
            return await _sftpService.CreateFileAsync(fileName, content);
        }

        public string ReadPollFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
