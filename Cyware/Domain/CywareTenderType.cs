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
    public class CywareTenderType : ITenderType<Model.TenderType>
    {

        private ISFTPService _sftpService { get; init; }

        public CywareTenderType(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public TenderType Add(TenderType tender)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int tenderCode)
        {
            throw new NotImplementedException();
        }

        public TenderType Get(int tenderCode)
        {
            throw new NotImplementedException();
        }

        public TenderType Get(string tenderCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(TenderType tender)
        {
            throw new NotImplementedException();
        }

        public TenderType Upsert(TenderType tender)
        {
            try
            {
                Validate(tender);
                TenderTypePOLL payload = new(tender);
                string formattedString = payload.Concat(payload);
                string filename = "POLL98.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                //if (!_sftpService.CreateLocal(filename, formattedString))
                {
                    return new Model.TenderType();
                }
                return tender;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.TenderType tenderType)
        {
            if (string.IsNullOrEmpty(tenderType.TenderTypeCd))
            {
                throw new ArgumentException("TenderTypeCd name is required.");
            }

            if (string.IsNullOrEmpty(tenderType.Description))
            {
                throw new ArgumentException("Description is required.");
            }
        }
    }
}
