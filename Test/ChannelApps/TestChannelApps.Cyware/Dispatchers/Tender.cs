using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Newtonsoft.Json;
using Moo.Models.Dtos.Items;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class Tender : Model.BaseTest
    {
        private readonly ITenderToQueue dispatcherToQueue;
        public Tender()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.Tender();
        }

        [Fact]
        public void Test_Tender_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetTenderDispatcher();
            // Deserialize the order data into a QueueMessageContent object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the data into specified models
            PaymentMethod d365_tender = JsonConvert.DeserializeObject<PaymentMethod>(payload.Body);

            KTI.Moo.Extensions.Cyware.Model.Tender tender = new()
            {
                tender_cd = d365_tender.TenderCode ?? "",
                tender_type_cd = d365_tender.PaymentType ?? "",
                description = d365_tender.Name ?? "",
                is_default = d365_tender.IsDefault ?? "",
                currency_cd = d365_tender.CurrencyCode ?? "",
                validation_spacing = d365_tender.ValidationSpacing ?? "",
                max_change = d365_tender.MaxChange ?? "",
                change_currency_code = d365_tender.ChangeCurrencyCode ?? "",
                mms_code = d365_tender.MMSCode ?? "",
                display_subtotal = "",
                min_amount = d365_tender.MinAmount ?? "",
                max_amount = d365_tender.MaxAmount ?? "",
                is_layaway_refund = "",
                max_refund = "",
                refund_type = "",
                is_mobile_payment = "",
                is_account = "",
                acct_type_code = d365_tender.AccountType ?? "",
                is_manager = "",
                garbage_tender_cd = "",
                rebate_tender_cd = "",
                rebate_percent = "",
                is_cashfund = "",
                is_takeout = "",
                item_code = "",
                surcharge_sku = "",
                mobile_payment_number = "",
                mobile_payment_return = "",
                is_padss = d365_tender.IsPADSS ?? "",
                is_credit_card = d365_tender.IsCreditCard ?? "",
                eft_port = "",
                is_voucher = d365_tender.IsVoucher ?? "",
                discount_cd = d365_tender.DiscountCode ?? ""
            };
            string jsonString = JsonConvert.SerializeObject(tender);


            // Act //
            bool result = Process(jsonString, tenderQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}
