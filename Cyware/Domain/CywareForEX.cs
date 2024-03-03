using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class ForEx : IForEx<Model.ForEx>
    {
        private ISFTPService _sftpService { get; init; }

        public ForEx(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Model.ForEx Add(Model.ForEx forexDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int currencyCode)
        {
            throw new NotImplementedException();
        }

        public Model.ForEx Get(int currencyCode)
        {
            throw new NotImplementedException();
        }

        public Model.ForEx Get(string currencyCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.ForEx forexDetails)
        {
            throw new NotImplementedException();
        }

        public Model.ForEx Upsert(Model.ForEx forexDetails)
        {
            try
            {
                //Validate(forexDetails);
                ForExPollEx payload = new(forexDetails);
                string formattedString = payload.Concat(payload);
                string filename = "POLLEX.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.ForEx();
                }
                return forexDetails;
            }
            catch (Exception ex)
            {
                return new Model.ForEx();
            }
        }

        //private static void Validate(Model.ForEx forEx)
        //{
        //    if (string.IsNullOrEmpty(forEx.from_currency_code))
        //    {
        //        throw new ArgumentException("From currency code is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.to_currency_code))
        //    {
        //        throw new ArgumentException("To currency code is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.currency_exch_rate))
        //    {
        //        throw new ArgumentException("Currency exchange rate is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.code))
        //    {
        //        throw new ArgumentException("Code is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.conversion_rate_type))
        //    {
        //        throw new ArgumentException("Conversion rate type is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.effective_date))
        //    {
        //        throw new ArgumentException("Effective date is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.rounding_multiple))
        //    {
        //        throw new ArgumentException("Rounding multiple is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.rounding_multiple_to))
        //    {
        //        throw new ArgumentException("Rounding multiple to is required.");
        //    }

        //    if (string.IsNullOrEmpty(forEx.currency_exch_rate_mt))
        //    {
        //        throw new ArgumentException("Currency exchange rate MT is required.");
        //    }
        //}
    }
}
