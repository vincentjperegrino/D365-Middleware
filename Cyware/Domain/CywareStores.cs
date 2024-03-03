using Kti.Moo.Cyware.Model.DTO;
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
    public class Stores : IStores<Store>
    {
        private ISFTPService _sftpService { get; init; }
        public Stores(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Store Add(Store storeDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int storeId)
        {
            throw new NotImplementedException();
        }

        public Store Get(int storeId)
        {
            throw new NotImplementedException();
        }

        public Store Get(string storeId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Store storeDetails)
        {
            throw new NotImplementedException();
        }

        public Store Upsert(Store storeDetails)
        {
            try
            {
                Validate(storeDetails);
                var poll64 = new POLL34DTO(storeDetails);
                string FormattedString = poll64.Concat(poll64);
                string filename = "POLL34.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new Store();
                }
                return storeDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.Store stores)
        {
            if (string.IsNullOrEmpty(stores.Name))
            {
                throw new ArgumentException("Store name is required.");
            }
        }
    }
}
