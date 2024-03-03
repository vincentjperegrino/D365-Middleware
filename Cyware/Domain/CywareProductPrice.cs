using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Core.Service;
using KTI.Moo.Extensions.Cyware.Services;

namespace KTI.Moo.Extensions.Cyware.Domain
{
    public class ProductPrice : IProductPrice<Model.ProductPrice>
    {
        private ISFTPService _sftpService { get; init; }

        public ProductPrice(Config config)
        {
            _sftpService = new Moo.Cyware.Services.SFTPService(config);
        }

        public Model.ProductPrice Add(Model.ProductPrice productPrice)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.ProductPrice Get(int productID)
        {
            throw new NotImplementedException();
        }

        public Model.ProductPrice Get(string productID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.ProductPrice productPrice)
        {
            throw new NotImplementedException();
        }

        public Model.ProductPrice Upsert(Model.ProductPrice productPrice)
        {
            try
            {
                Validate(productPrice);
                ProductPricePoll79 payload = new(productPrice);
                string formattedString = payload.Concat(payload);
                string filename = "POLL79.DWN";
                if (!_sftpService.CreateFile(filename, formattedString))
                {
                    return new Model.ProductPrice();
                }
                return productPrice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Validate(Model.ProductPrice productPrice)
        {
            if (string.IsNullOrEmpty(productPrice.sku_number))
            {
                throw new ArgumentException("SKU number is required.");
            }

            //if (int.IsNullOrEmpty(productPrice.upc_code))
            //{
            //    throw new ArgumentException("UPC code is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.upc_type))
            //{
            //    throw new ArgumentException("UPC type is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.price_event_number))
            //{
            //    throw new ArgumentException("Price event number is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.currency_code))
            //{
            //    throw new ArgumentException("Currency code is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.price_book))
            //{
            //    throw new ArgumentException("Price book is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.start_date))
            //{
            //    throw new ArgumentException("Start date is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.end_date))
            //{
            //    throw new ArgumentException("End date is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.promo_flag_yn))
            //{
            //    throw new ArgumentException("Promo flag is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.event_price_multiple))
            //{
            //    throw new ArgumentException("Event price multiple is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.event_price))
            //{
            //    throw new ArgumentException("Event price is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.price_method_code))
            //{
            //    throw new ArgumentException("Price method code is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.mix_match_code))
            //{
            //    throw new ArgumentException("Mix match code is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.deal_quantity))
            //{
            //    throw new ArgumentException("Deal quantity is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.deal_price))
            //{
            //    throw new ArgumentException("Deal price is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.buy_quantity))
            //{
            //    throw new ArgumentException("Buy quantity is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.buy_value))
            //{
            //    throw new ArgumentException("Buy value is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.buy_value_type))
            //{
            //    throw new ArgumentException("Buy value type is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.qty_end_value))
            //{
            //    throw new ArgumentException("Quantity end value is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.quantity_break))
            //{
            //    throw new ArgumentException("Quantity break is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.quantity_group_price))
            //{
            //    throw new ArgumentException("Quantity group price is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.quantity_unit_price))
            //{
            //    throw new ArgumentException("Quantity unit price is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.cust_promo_code))
            //{
            //    throw new ArgumentException("Customer promo code is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.cust_number))
            //{
            //    throw new ArgumentException("Customer number is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.precedence_level))
            //{
            //    throw new ArgumentException("Precedence level is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.default_currency))
            //{
            //    throw new ArgumentException("Default currency is required.");
            //}

            //if (string.IsNullOrEmpty(productPrice.default_price_book))
            //{
            //    throw new ArgumentException("Default price book is required.");
            //}
        }
    }
}
