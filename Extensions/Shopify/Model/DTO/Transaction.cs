using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Transaction : ShopifySharp.Transaction
    {
        public Transaction()
        {

        }


        public Transaction(Model.Transaction transaction)
        {
            Amount = transaction.amount;
            Authorization = transaction.authorization;
            if (transaction.authorization_expires_at != default)
            {
                AuthorizationExpiresAt = transaction.authorization_expires_at;
            }
            if (transaction.created_at != default)
            {
                CreatedAt = transaction.created_at;
            }
            DeviceId = transaction.device_id;
            Gateway = transaction.gateway;
            SourceName = transaction.source_name;
            Source = transaction.source;
            if (transaction.payment_details != null) 
            {
                PaymentDetails = new Model.DTO.PaymentDetails(transaction.payment_details);
            }
            Kind = transaction.kind;
            OrderId = transaction.order_id;
            Receipt = transaction.receipt;
            ErrorCode = transaction.error_code;
            Status = transaction.status;
            Test = transaction.test;
            UserId = transaction.user_id;
            Currency = transaction.currency;
            Message = transaction.message;
            LocationId = transaction.location_id;
            ParentId = transaction.parent_id;
            if (transaction.processed_at != default)
            {
                ProcessedAt = transaction.processed_at;
            }
            MaximumRefundable = transaction.maximum_refundable;
            if (transaction.currency_exchange_adjustment != null)
            {
                CurrencyExchangeAdjustment = new Model.DTO.CurrencyExchangeAdjustment(transaction.currency_exchange_adjustment);
            }
            if (transaction.payments_refund_attributes != null)
            {
                PaymentsRefundAttributes = new Model.DTO.PaymentRefundAttributes(transaction.payments_refund_attributes);
            }
            PaymentId = transaction.payment_id;
            if (transaction.total_unsettled_set != null)
            {

                TotalUnsettledSet = new Model.DTO.PriceSet(transaction.total_unsettled_set);


            }






        }
        public new string SourceName { get; set; }

 
    }



}
