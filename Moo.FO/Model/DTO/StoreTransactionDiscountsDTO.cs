using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace Moo.FO.Model.DTO
{
    public class StoreTransactionDiscountsDTO : StoreTransactionDiscounts
    {
        public StoreTransactionDiscountsDTO(FOSalesTransactionDiscount transactionDiscount, OrdersTransaction ordersTransaction)
        {
            Validate(transactionDiscount);
            string storeNumber = ordersTransaction.Header.ChannelReferenceId;

            this.dataAreaId = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == storeNumber).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? throw new Exception("dataAreaId is null or empty");
            this.TransactionNumber = transactionDiscount.TransactionNumber;
            this.Terminal = transactionDiscount.Terminal.Length < 6 ? transactionDiscount.Terminal.PadLeft(6, '0') : transactionDiscount.Terminal;
            this.OperatingUnitNumber = ordersTransaction.Config.RetailStores.Where(rs => rs.StoreNumber == ordersTransaction.Header.ChannelReferenceId).Select(rs => rs.OperatingUnitNumber).FirstOrDefault() ?? throw new Exception("OperatingUnitNumber is null or empty");
            this.LineNumber = int.Parse(transactionDiscount.LineNumber);
            this.SalesLineNumber = int.Parse(transactionDiscount.SalesLineNumber);
            this.DiscountAmount = Decimal.Parse(transactionDiscount.DiscountAmount);
            this.BundleId = 0;
            this.CustomerDiscountType = transactionDiscount.CustomerDiscountType;
            this.DiscountCode = "";
            this.DiscountCost = 0;
            this.ManualDiscountType = "None";
            this.DiscountOfferId = transactionDiscount.DiscountOfferId;
            this.DiscountPercentage = 0;
            this.EffectiveAmount = Decimal.Parse(transactionDiscount.DiscountAmount);
            this.DiscountOriginType = "None";
            this.DealPrice = 0;
        }

        private static void Validate(FOSalesTransactionDiscount discount)
        {
            if (string.IsNullOrEmpty(discount.Terminal))
            {
                throw new ArgumentException("Terminal", "Terminal is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.TransactionNumber))
            {
                throw new ArgumentException("TransactionNumber", "TransactionNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.SalesLineNumber))
            {
                throw new ArgumentException("SalesLineNumber", "SalesLineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.LineNumber))
            {
                throw new ArgumentException("LineNumber", "LineNumber is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.DiscountAmount))
            {
                throw new ArgumentException("DiscountAmount", "DiscountAmount is null or empty.");
            }

            if (string.IsNullOrEmpty(discount.EffectiveAmount))
            {
                throw new ArgumentException("EffectiveAmount", "EffectiveAmount is null or empty.");
            }
        }

        public StoreTransactionDiscountsDTO() { }

        //public StoreTransactionDiscountsDTO(Extensions.Cyware.App.Receiver.Models.SalesTransactionDiscount transactionDiscount, int lineNumber, D365FOConfig config)
        //{
        //    Validate(transactionDiscount);
        //    bool returnTrans = false;

        //    string discType = "0";

        //    switch (transactionDiscount.DiscTransType)
        //    {
        //        case "CA":
        //            discType = "2";
        //            break;
        //        case "EP":
        //            discType = "17";
        //            break;
        //    }

        //    this.dataAreaId = config.RetailStores.Where(rs => rs.StoreNumber == transactionDiscount.StoreNumber && !string.IsNullOrEmpty(rs.DefaultCustomerLegalEntity)).Select(rs => rs.DefaultCustomerLegalEntity).FirstOrDefault() ?? ""; // direct
        //    this.TransactionNumber = transactionDiscount.TransNumber;
        //    this.Terminal = transactionDiscount.RegisterID; // direct
        //    this.OperatingUnitNumber = transactionDiscount.StoreNumber; // direct
        //    this.LineNumber = lineNumber;
        //    this.SalesLineNumber = lineNumber; // direct, same with line number
        //    this.DiscountAmount = decimal.Parse(transactionDiscount.TransDiscAmount, CultureInfo.InvariantCulture);
        //    this.BundleId = 0; // optional
        //    this.CustomerDiscountType = String.IsNullOrEmpty(discType) ? "None" : discType; // ?
        //    this.DiscountCode = String.IsNullOrEmpty(transactionDiscount.DiscReasonType) ? "None" : transactionDiscount.DiscReasonType;
        //    this.DiscountCost = 0; // optional
        //    this.ManualDiscountType = "None"; // direct/config
        //    this.DiscountOfferId = String.IsNullOrEmpty(discType) ? "None" : discType; // ?
        //    this.DiscountPercentage = 0; // direct, ((discount amount / (unit retail * qty)) * 100)
        //    this.EffectiveAmount = Decimal.Parse(transactionDiscount.DiscountAmount); // discount amount
        //    this.DiscountOriginType = "None"; // direct/config
        //    this.DealPrice = 0; // optional
        //}

        //private static void Validate(SalesTransactionDiscount transactionDiscount)
        //{
        //    if (string.IsNullOrEmpty(transactionDiscount.TransNumber))
        //    {
        //        throw new ArgumentException("TransNumber", "TransNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDiscount.RegisterID))
        //    {
        //        throw new ArgumentException("RegisterID", "RegisterID is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDiscount.TransDiscAmount))
        //    {
        //        throw new ArgumentException("TransDiscAmount", "TransDiscAmount is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDiscount.StoreNumber))
        //    {
        //        throw new ArgumentException("StoreNumber", "StoreNumber is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDiscount.DiscReasonType))
        //    {
        //        throw new ArgumentException("DiscReasonType", "DiscReasonType is null or empty.");
        //    }
        //    if (string.IsNullOrEmpty(transactionDiscount.DiscountAmount))
        //    {
        //        throw new ArgumentException("DiscountAmount", "DiscountAmount is null or empty.");
        //    }
        //}
    }
}
