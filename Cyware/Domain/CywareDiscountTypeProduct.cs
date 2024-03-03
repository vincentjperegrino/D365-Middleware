using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class CywareDiscountTypeProduct : IDiscountTypeProduct<Model.DiscountTypeProduct>
    {

        private ISFTPService _sftpService { get; init; }

        public CywareDiscountTypeProduct(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }
        public DiscountTypeProduct Add(DiscountTypeProduct discountTypeDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public DiscountTypeProduct Get(int discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public DiscountTypeProduct Get(string discountTypeCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(DiscountTypeProduct discountTypeDetails)
        {
            throw new NotImplementedException();
        }

        public DiscountTypeProduct Upsert(DiscountTypeProduct discountTypeDetails)
        {
            try
            {
                var discountTypeProductPOLL = new DiscountTypeProductPOLL(discountTypeDetails);
                string FormattedString = discountTypeProductPOLL.Concat(discountTypeProductPOLL);
                string filename = "POLL100.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.DiscountTypeProduct();
                }
                return new Model.DiscountTypeProduct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
