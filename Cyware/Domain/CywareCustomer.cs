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
    public class Customer : ICustomer<KTI.Moo.Extensions.Cyware.Model.Customer>
    {
        private ISFTPService _sftpService { get; init; }

        public Customer(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public Model.Customer Add(Model.Customer customerDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int customerID)
        {
            throw new NotImplementedException();
        }

        public Model.Customer Get(int customerID)
        {
            throw new NotImplementedException();
        }

        public Model.Customer Get(string customerID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.Customer customerDetails)
        {
            throw new NotImplementedException();
        }

        public Model.Customer Upsert(Model.Customer customerDetails)
        {
            try
            {
                Validate(customerDetails);
                var customerPOLL = new CustomerPOLL(customerDetails);
                string FormattedString = customerPOLL.Concat(customerPOLL);

                
                string filename = "POLL96.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Model.Customer();
                }
                return new Model.Customer(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.Customer customer)
        {
            if (string.IsNullOrEmpty(customer.CustomerId))
            {
                throw new ArgumentException("CustomerId is required.");
            }

            if (string.IsNullOrEmpty(customer.Name))
            {
                throw new ArgumentException("CustomerName is required.");
            }
        }
    }
}
