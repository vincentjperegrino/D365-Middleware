using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class CywareBOMLines : IBOMLines<KTI.Moo.Extensions.Cyware.Model.BOMLines>
    {
        private ISFTPService _sftpService { get; init; }

        public CywareBOMLines(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public BOMLines Add(BOMLines bomDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMLines Get(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMLines Get(string bomID)
        {
            throw new NotImplementedException();
        }

        public bool Update(BOMLines bomDetails)
        {
            throw new NotImplementedException();
        }

        public BOMLines Upsert(BOMLines bomDetails)
        {
            try
            {
                //Validate(bomDetails);
                var bomHeaderPOLL = new BOMLinesPOLL(bomDetails);
                string FormattedString = bomHeaderPOLL.Concat(bomHeaderPOLL);

                string filename = "POLL105.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.BOMLines();
                }
                return new Model.BOMLines();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
