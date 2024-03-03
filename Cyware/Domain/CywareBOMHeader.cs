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
    public class CywareBOMHeader : IBOMHeader<KTI.Moo.Extensions.Cyware.Model.BOMHeader>
    {
        private ISFTPService _sftpService { get; init; }

        public CywareBOMHeader(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }


        public BOMHeader Add(BOMHeader bomDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMHeader Get(int bomID)
        {
            throw new NotImplementedException();
        }

        public BOMHeader Get(string bomID)
        {
            throw new NotImplementedException();
        }

        public bool Update(BOMHeader bomDetails)
        {
            throw new NotImplementedException();
        }

        public BOMHeader Upsert(BOMHeader bomDetails)
        {
            try
            {
                //Validate(bomDetails);
                var bomHeaderPOLL = new BOMHeaderPOLL(bomDetails);
                string FormattedString = bomHeaderPOLL.Concat(bomHeaderPOLL);


                string filename = "POLL103.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.BOMHeader();
                }
                return new Model.BOMHeader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
