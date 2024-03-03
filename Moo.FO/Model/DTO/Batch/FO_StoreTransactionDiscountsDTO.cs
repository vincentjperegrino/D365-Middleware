using KTI.Moo.Extensions.Cyware.App.Receiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moo.FO.Model.DTO.Batch
{
    public class FO_StoreDiscountsDTO : StoreTransactionDiscounts
    {
        private static string errorPrefix = "";
        public FO_StoreDiscountsDTO(FOSalesTransactionDiscount discount, D365FOConfig config)
        {
            Validate(discount);
            try
            {
                string storeNumber = "";
                string register = discount.Terminal;
                if (discount.Terminal.Contains('-'))
                {
                    string[] storeAndRegister = register.Split('-');
                    storeNumber = storeAndRegister[0];
                    register = storeAndRegister[1];
                }
                this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, dataAreaId is null or empty");
                this.TransactionNumber = discount.TransactionNumber;
                this.Terminal = config.Terminals.Where(rt => rt.StoreNumber == storeNumber && rt.Name == register).Select(rt => rt.TerminalNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, Terminal is null or empty");
                this.OperatingUnitNumber = config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception($"{errorPrefix}, OperatingUnitNumber is null or empty");
                this.LineNumber = int.Parse(discount.LineNumber);
                this.SalesLineNumber = int.Parse(discount.SalesLineNumber);
                this.DiscountAmount = Decimal.Parse(discount.DiscountAmount);
                this.BundleId = 0;
                this.CustomerDiscountType = discount.CustomerDiscountType;
                this.DiscountCode = "";
                this.DiscountCost = 0;
                this.ManualDiscountType = "None";
                this.DiscountOfferId = discount.DiscountOfferId;
                this.DiscountPercentage = 0;
                this.EffectiveAmount = Decimal.Parse(discount.DiscountAmount);
                this.DiscountOriginType = "None";
                this.DealPrice = 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"{errorPrefix}, {ex.Message}");
            }
        }

        private static void Validate(FOSalesTransactionDiscount discount)
        {
            if (string.IsNullOrEmpty(discount.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }
            errorPrefix = $"Discounts Transaction Number: {discount.TransactionNumber}";

            if (string.IsNullOrEmpty(discount.Terminal))
            {
                throw new ArgumentException("Terminal", $"{errorPrefix}, Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.SalesLineNumber))
            {
                throw new ArgumentException("SalesLineNumber", $"{errorPrefix}, SalesLineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.LineNumber))
            {
                throw new ArgumentException("LineNumber", $"{errorPrefix}, LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.DiscountAmount))
            {
                throw new ArgumentException("DiscountAmount", $"{errorPrefix}, DiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.EffectiveAmount))
            {
                throw new ArgumentException("EffectiveAmount", $"{errorPrefix}, EffectiveAmount is null or empty.");
            }
        }

        public override string ToString()
        {
            // Using Newtonsoft.Json to serialize the object to JSON
            return JsonConvert.SerializeObject(this);
        }
    }
}
