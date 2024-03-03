using Newtonsoft.Json;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Transaction {

    public Transaction() { }

    public Transaction(ShopifySharp.Transaction transaction) {

        amount = transaction.Amount ?? default;
        authorization = transaction.Authorization;
        authorization_expires_at = transaction.AuthorizationExpiresAt ?? default;
        created_at = transaction.CreatedAt ?? default;
        device_id = transaction.DeviceId ?? default;
        gateway = transaction.Gateway;
        source_name = transaction.SourceName;
        source = transaction.Source;
        if (transaction.PaymentDetails != null)
        {
            payment_details = new Model.PaymentDetails(transaction.PaymentDetails);

        }
        kind = transaction.Kind;
        order_id = transaction.OrderId ?? default;
        receipt = transaction.Receipt;
        error_code = transaction.ErrorCode;
        status = transaction.Status;
        test = transaction.Test ?? default;
        user_id = transaction.UserId ?? default;
        currency = transaction.Currency;
        message = transaction.Message;
        location_id = transaction.LocationId ?? default;
        parent_id = transaction.ParentId ?? default;
        processed_at = transaction.ProcessedAt ?? default;
        maximum_refundable = transaction.MaximumRefundable ?? default;

        if (transaction.CurrencyExchangeAdjustment != null) {

            currency_exchange_adjustment = new Model.CurrencyExchangeAdjustment(transaction.CurrencyExchangeAdjustment);
        }

        if (transaction.PaymentsRefundAttributes != null)
        {
            payments_refund_attributes = new Model.PaymentsRefundAttributes(transaction.PaymentsRefundAttributes);


        }

        payment_id = transaction.PaymentId;

        if (transaction.TotalUnsettledSet != null)
        {

            total_unsettled_set = new Model.PriceSet(transaction.TotalUnsettledSet);



        }








    }


    public decimal amount { get; set; }
    public string authorization { get; set; }
    public DateTimeOffset authorization_expires_at { get; set; }
    public DateTimeOffset created_at { get; set; }
    public long device_id { get; set; }
    public string gateway { get; set; }
    public string source_name { get; private set; }
    public string source { get; set; }
    public PaymentDetails payment_details { get; set; }
    public string kind { get; set; }
    public long order_id { get; set; }
    public object receipt { get; set; }
    public string error_code { get; set; }
    public string status { get; set; }
    public bool test { get; set; }
    public long user_id { get; set; }
    public string currency { get; set; }
    public string message { get; set; }
    public long location_id { get; set; }
    public long parent_id { get; set; }
    public DateTimeOffset processed_at { get; set; }
    public decimal maximum_refundable { get; set; }
    public CurrencyExchangeAdjustment currency_exchange_adjustment { get; set; }
    public PaymentsRefundAttributes payments_refund_attributes { get; set; }
    public string payment_id { get; set; }
    public PriceSet total_unsettled_set { get; set; }

}
