using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Cyware.Services;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class Discounts : IDiscount<Discount>
    {
        private ISFTPService _sftpService { get; init; }

        public Discounts(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public Discount Add(Discount disountHeader)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int discountId)
        {
            throw new NotImplementedException();
        }

        public Discount Get(int discountId)
        {
            throw new NotImplementedException();
        }

        public Discount Get(string discountId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Discount discountHeader)
        {
            throw new NotImplementedException();
        }

        public Discount Upsert(Discount discountHeader)
        {
            try
            {
                var poll64 = new POLL64DTO(discountHeader.evtNum, discountHeader.evtDsc, DateTime.Parse(discountHeader.evtFdt.ToString()), DateTime.Parse(discountHeader.evtTdt.ToString()));
                string FormattedString = poll64.Concat(poll64);
                string filename = "POLL64.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Discount();
                }
                return discountHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
