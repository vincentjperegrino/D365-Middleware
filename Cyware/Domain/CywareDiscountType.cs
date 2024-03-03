using KTI.Moo.Cyware.Model.DTO;
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
    public class CywareDiscountType : IDiscountType<Model.DiscountType>
    {

        private ISFTPService _sftpService { get; init; }

        public CywareDiscountType(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public DiscountType Add(DiscountType discountTypeDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public DiscountType Get(int discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public DiscountType Get(string discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(DiscountType discountTypeDetails)
        {
            throw new NotImplementedException();
        }

        public DiscountType Upsert(DiscountType discountTypeDetails)
        {
            try
            {
                var customerPOLL = new DiscountTypePoll(discountTypeDetails);
                string FormattedString = customerPOLL.Concat(customerPOLL);
                string filename = "POLL99.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.DiscountType();
                }
                return new Model.DiscountType();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
