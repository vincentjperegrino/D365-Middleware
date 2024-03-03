using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;
using System.Data;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class Products : IProducts<Model.Products>
    {
        private ISFTPService _sftpService { get; init; }

        public Products(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Model.Products Add(Model.Products productDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.Products Get(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.Products Get(string productID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.Products productDetails)
        {
            throw new NotImplementedException();
        }

        public Model.Products Upsert(Model.Products productDetails, string FileName)
        {
            try
            {
                Validate(productDetails);
                ProductInitialPOLL53 payload = new(productDetails);
                string formattedString = payload.Concat(payload);
                string filename = FileName;
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.Products();
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.Products products)
        {

            if (string.IsNullOrEmpty(products.department))
            {
                throw new ArgumentException("Product Category Department is required.");
            }

            if (string.IsNullOrEmpty(products.sub_department))
            {
                throw new ArgumentException("Product Category Sub_Department is required.");
            }

            if (string.IsNullOrEmpty(products.cy_class))
            {
                throw new ArgumentException("Product Category Class is required.");
            }

            if (string.IsNullOrEmpty(products.sub_class))
            {
                throw new ArgumentException("Product Category Sub_Class is required.");
            }

            if (string.IsNullOrEmpty(products.item_description))
            {
                throw new ArgumentException("Product item_description is required.");
            }
        }
    }
}
