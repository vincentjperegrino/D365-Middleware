using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Services;


namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class Prices : IPrices<PriceHeader>
    {
        private ISFTPService _sftpService { get; init; }

        public Prices(Config config)
        {
            _sftpService = new KTI.Moo.Cyware.Services.SFTPService(config);
        }

        public PriceHeader Add(PriceHeader customerDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int customerID)
        {
            throw new NotImplementedException();
        }

        public PriceHeader Get(int customerID)
        {
            throw new NotImplementedException();
        }

        public PriceHeader Get(string customerID)
        {
            throw new NotImplementedException();
        }

        public bool Update(PriceHeader customerDetails)
        {
            throw new NotImplementedException();
        }

        public PriceHeader Upsert(PriceHeader priceHeader)
        {
            try
            {
                DateTime? startDate = priceHeader.evtFdt?.ToString() != null ? DateTime.TryParse(priceHeader.evtFdt.ToString(), out DateTime sDate) ? sDate : (DateTime?)null : null;
                DateTime? endDate = priceHeader.evtFdt?.ToString() != null ? DateTime.TryParse(priceHeader.evtTdt.ToString(), out DateTime eDate) ? eDate : (DateTime?)null : null;

                var poll64 = new POLL64DTO(priceHeader.evtNum ?? "", priceHeader.evtDsc ?? "", startDate, endDate);
                string FormattedString = poll64.Concat(poll64);
                string filename = "POLL64.DWN";
                if (!_sftpService.CreateFile(filename, FormattedString))
                {
                    return new PriceHeader();
                }
                return priceHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

