using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class CywareDiscountLocation : IDiscountLocation<DiscountLocation>
    {
        private ISFTPService _sftpService { get; init; }

        public CywareDiscountLocation(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public DiscountLocation Add(DiscountLocation discountLocation)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int discountLocationId)
        {
            throw new NotImplementedException();
        }

        public DiscountLocation Get(int discountLocationId)
        {
            throw new NotImplementedException();
        }

        public DiscountLocation Get(string discountLocationId)
        {
            throw new NotImplementedException();
        }

        public bool Update(DiscountLocation discountLocation)
        {
            throw new NotImplementedException();
        }

        public DiscountLocation Upsert(DiscountLocation discountLocation)
        {
            try
            {
                var discLocationPOLL = new DiscountLocationPOLL(discountLocation);
                string FormattedString = discLocationPOLL.Concat(discLocationPOLL);
                string filename = "POLL101.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new DiscountLocation();
                }
                return discountLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
