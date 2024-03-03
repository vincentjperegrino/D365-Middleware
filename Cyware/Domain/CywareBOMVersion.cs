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
    public class CywareBOMVersion : IBOMVersion<KTI.Moo.Extensions.Cyware.Model.BOMVersion>
    {
        private ISFTPService _sftpService { get; init; }

        public CywareBOMVersion(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }
        public BOMVersion Add(BOMVersion bomDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMVersion Get(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMVersion Get(string bomID)
        {
            throw new NotImplementedException();
        }

        public bool Update(BOMVersion bomDetails)
        {
            throw new NotImplementedException();
        }

        public BOMVersion Upsert(BOMVersion bomDetails)
        {
            try
            {
                //Validate(bomDetails);
                var bomHeaderPOLL = new BOMVersionPOLL(bomDetails);
                string FormattedString = bomHeaderPOLL.Concat(bomHeaderPOLL);

                string filename = "POLL104.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.BOMVersion();
                }
                return new Model.BOMVersion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
