using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class ProductBarcode : IProductBarcode<Model.ProductBarcode>
    {
        private ISFTPService _sftpService { get; init; }

        public ProductBarcode(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Model.ProductBarcode Add(Model.ProductBarcode productBarcode)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.ProductBarcode Get(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.ProductBarcode Get(string productID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.ProductBarcode productBarcode)
        {
            throw new NotImplementedException();
        }

        public Model.ProductBarcode Upsert(Model.ProductBarcode productBarcode)
        {
            try
            {
                Validate(productBarcode);
                ProductBarcodePoll54 payload = new(productBarcode);
                string formattedString = payload.Concat(payload);
                string filename = "POLL56.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.ProductBarcode();
                }
                return productBarcode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.ProductBarcode productBarcode)
        {
            if (string.IsNullOrEmpty(productBarcode.product_code))
            {
                throw new ArgumentException("Product code is required.");
            }

            if (string.IsNullOrEmpty(productBarcode.sku_number))
            {
                throw new ArgumentException("SKU number is required.");
            }

            if (string.IsNullOrEmpty(productBarcode.upc_unit_of_measure))
            {
                throw new ArgumentException("UPC unit of measure is required.");
            }
        }
    }
}
