using KTI.Moo.Cyware.Model.DTO;
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
    public class ProductCategory : IProductCategory<KTI.Moo.Extensions.Cyware.Model.ProductCategory>
    {
        private ISFTPService _sftpService { get; init; }
        public ProductCategory(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public Model.ProductCategory Add(Model.ProductCategory productCategoryDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productCategoryId)
        {
            throw new NotImplementedException();
        }

        public Model.ProductCategory Get(int productCategoryId)
        {
            throw new NotImplementedException();
        }

        public Model.ProductCategory Get(string productCategoryId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.ProductCategory productCategoryDetails)
        {
            throw new NotImplementedException();
        }

        public Model.ProductCategory Upsert(Model.ProductCategory productCategoryDetails)
        {
            try
            {
                Validate(productCategoryDetails);
                var poll54 = new ProductCategoryPoll54(productCategoryDetails);
                string FormattedString = poll54.Concat(poll54);
                string filename = "POLL54.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.ProductCategory();
                }
                return productCategoryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.ProductCategory productCategory)
        {
            if (string.IsNullOrEmpty(productCategory.department))
            {
                throw new ArgumentException("Department cannot be null.");
            }
        }
    }
}
