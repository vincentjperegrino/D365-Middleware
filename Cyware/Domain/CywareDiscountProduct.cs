using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class DiscountProduct : IDiscountProduct<Model.DiscountProduct>
    {
        private ISFTPService _sftpService { get; init; }

        public DiscountProduct(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Model.DiscountProduct Add(Model.DiscountProduct discountDetails)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int discountCode)
        {
            throw new NotImplementedException();
        }

        public Model.DiscountProduct Get(int discountCode)
        {
            throw new NotImplementedException();
        }

        public Model.DiscountProduct Get(string discountCode)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.DiscountProduct discountDetails)
        {
            throw new NotImplementedException();
        }

        public Model.DiscountProduct Upsert(Model.DiscountProduct discountDetails)
        {
            try
            {
                //Validate(discountDetails);
                DiscountProductPoll79 payload = new(discountDetails);
                string formattedString = payload.Concat(payload);
                string filename = "POLL79.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.DiscountProduct();
                }
                return discountDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static void Validate(Model.DiscountProduct discountProduct) 
        //{
        //    if (string.IsNullOrEmpty(discountProduct.sku_number))
        //    {
        //        throw new ArgumentException("SKU number is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.upc_code))
        //    {
        //        throw new ArgumentException("UPC code is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.upc_type))
        //    {
        //        throw new ArgumentException("UPC type is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.price_event_number))
        //    {
        //        throw new ArgumentException("Price event number is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.currency_code))
        //    {
        //        throw new ArgumentException("Currency code is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.price_book))
        //    {
        //        throw new ArgumentException("Price book is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.start_date))
        //    {
        //        throw new ArgumentException("Start date is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.end_date))
        //    {
        //        throw new ArgumentException("End date is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.promo_flag_yn))
        //    {
        //        throw new ArgumentException("Promo flag is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.event_price_multiple))
        //    {
        //        throw new ArgumentException("Event price multiple is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.event_price))
        //    {
        //        throw new ArgumentException("Event price is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.price_method_code))
        //    {
        //        throw new ArgumentException("Price method code is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.mix_match_code))
        //    {
        //        throw new ArgumentException("Mix match code is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.deal_quantity))
        //    {
        //        throw new ArgumentException("Deal quantity is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.deal_price))
        //    {
        //        throw new ArgumentException("Deal price is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.buy_quantity))
        //    {
        //        throw new ArgumentException("Buy quantity is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.buy_value))
        //    {
        //        throw new ArgumentException("Buy value is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.buy_value_type))
        //    {
        //        throw new ArgumentException("Buy value type is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.qty_end_value))
        //    {
        //        throw new ArgumentException("Quantity end value is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.quantity_break))
        //    {
        //        throw new ArgumentException("Quantity break is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.quantity_group_price))
        //    {
        //        throw new ArgumentException("Quantity group price is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.quantity_unit_price))
        //    {
        //        throw new ArgumentException("Quantity unit price is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.cust_promo_code))
        //    {
        //        throw new ArgumentException("Customer promo code is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.cust_number))
        //    {
        //        throw new ArgumentException("Customer number is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.precedence_level))
        //    {
        //        throw new ArgumentException("Precedence level is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.default_currency))
        //    {
        //        throw new ArgumentException("Default currency is required.");
        //    }

        //    if (string.IsNullOrEmpty(discountProduct.default_price_book))
        //    {
        //        throw new ArgumentException("Default price book is required.");
        //    }

        //}
    }
}
