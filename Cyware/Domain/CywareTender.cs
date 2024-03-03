using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Model;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class Tender : ITender<Model.Tender>
    {
        private ISFTPService _sftpService { get; init; }

        public Tender(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }
        public Model.Tender Add(Model.Tender tender)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int tenderCode)
        {
            throw new NotImplementedException();
        }

        public Model.Tender Get(int tenderCode)
        {
            throw new NotImplementedException();
        }

        public Model.Tender Get(string tenderCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.Tender tender)
        {
            throw new NotImplementedException();
        }

        public Model.Tender Upsert(Model.Tender tender)
        {
            try
            {
                //Validate(tender);
                TenderPoll97 payload = new(tender);
                string formattedString = payload.Concat(payload);
                string filename = "POLL97.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.Tender();
                }
                return tender;
            }
            catch (Exception ex)
            {
                return new Model.Tender();
            }
        }

        private static void Validate(Model.Tender tender)
        {
            if (string.IsNullOrEmpty(tender.tender_cd))
            {
                throw new ArgumentException("Tender code is required.");
            }

            if (string.IsNullOrEmpty(tender.tender_type_cd))
            {
                throw new ArgumentException("Tender type code is required.");
            }

            if (string.IsNullOrEmpty(tender.description))
            {
                throw new ArgumentException("Description is required.");
            }

            if (string.IsNullOrEmpty(tender.is_default))
            {
                throw new ArgumentException("Is default is required.");
            }

            if (string.IsNullOrEmpty(tender.currency_cd))
            {
                throw new ArgumentException("Currency code is required.");
            }

            if (string.IsNullOrEmpty(tender.validation_spacing))
            {
                throw new ArgumentException("Validation spacing is required.");
            }

            if (string.IsNullOrEmpty(tender.max_change))
            {
                throw new ArgumentException("Maximum change is required.");
            }

            if (string.IsNullOrEmpty(tender.change_currency_code))
            {
                throw new ArgumentException("Change currency code is required.");
            }

            if (string.IsNullOrEmpty(tender.mms_code))
            {
                throw new ArgumentException("MMS code is required.");
            }

            if (string.IsNullOrEmpty(tender.display_subtotal))
            {
                throw new ArgumentException("Display subtotal is required.");
            }

            if (string.IsNullOrEmpty(tender.min_amount))
            {
                throw new ArgumentException("Minimum amount is required.");
            }

            if (string.IsNullOrEmpty(tender.max_amount))
            {
                throw new ArgumentException("Maximum amount is required.");
            }

            if (string.IsNullOrEmpty(tender.is_layaway_refund))
            {
                throw new ArgumentException("Is layaway refund is required.");
            }

            if (string.IsNullOrEmpty(tender.max_refund))
            {
                throw new ArgumentException("Maximun refund is required.");
            }

            if (string.IsNullOrEmpty(tender.refund_type))
            {
                throw new ArgumentException("Refund type is required.");
            }

            if (string.IsNullOrEmpty(tender.is_mobile_payment))
            {
                throw new ArgumentException("Is mobile payment is required.");
            }

            if (string.IsNullOrEmpty(tender.is_account))
            {
                throw new ArgumentException("Is account is required.");
            }

            if (string.IsNullOrEmpty(tender.acct_type_code))
            {
                throw new ArgumentException("Account type code is required.");
            }

            if (string.IsNullOrEmpty(tender.is_manager))
            {
                throw new ArgumentException("Is manager is required.");
            }

            if (string.IsNullOrEmpty(tender.garbage_tender_cd))
            {
                throw new ArgumentException("Garbage tender code is required.");
            }

            if (string.IsNullOrEmpty(tender.rebate_tender_cd))
            {
                throw new ArgumentException("Rebate tender code is required.");
            }

            if (string.IsNullOrEmpty(tender.rebate_percent))
            {
                throw new ArgumentException("Rebate percent is required.");
            }

            if (string.IsNullOrEmpty(tender.is_cashfund))
            {
                throw new ArgumentException("Is cash fund is required.");
            }

            if (string.IsNullOrEmpty(tender.is_takeout))
            {
                throw new ArgumentException("Is take out is required.");
            }

            if (string.IsNullOrEmpty(tender.item_code))
            {
                throw new ArgumentException("Precedence level is required.");
            }

            if (string.IsNullOrEmpty(tender.surcharge_sku))
            {
                throw new ArgumentException("Surcharge SKU is required.");
            }

            if (string.IsNullOrEmpty(tender.mobile_payment_number))
            {
                throw new ArgumentException("Mobile payment number is required.");
            }
            if (string.IsNullOrEmpty(tender.mobile_payment_return))
            {
                throw new ArgumentException("Mobile payment return is required.");
            }

            if (string.IsNullOrEmpty(tender.is_padss))
            {
                throw new ArgumentException("Is PADSS is required.");
            }

            if (string.IsNullOrEmpty(tender.is_credit_card))
            {
                throw new ArgumentException("Is credit card is required.");
            }

            if (string.IsNullOrEmpty(tender.eft_port))
            {
                throw new ArgumentException("EFT Port is required.");
            }

            if (string.IsNullOrEmpty(tender.is_voucher))
            {
                throw new ArgumentException("Is voucher is required.");
            }

            if (string.IsNullOrEmpty(tender.discount_cd))
            {
                throw new ArgumentException("Discount code is required.");
            }
        }
    }
}
