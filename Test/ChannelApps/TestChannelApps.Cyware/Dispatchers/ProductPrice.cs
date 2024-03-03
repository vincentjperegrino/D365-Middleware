using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using KTI.Moo.Extensions.Cyware.Model.DTO;
using Moo.Models.Dtos.Items;
using Newtonsoft.Json;

namespace TestChannelApps.Cyware.Dispatchers
{
    public class ProductPrice : Model.BaseTest
    {
        private readonly IProductPriceToQueue dispatcherToQueue;
        public ProductPrice()
        {
            dispatcherToQueue = new KTI.Moo.ChannelApps.Model.RDF.Dispatchers.ProductPrice();
        }

        [Fact]
        public void Test_ProductPrice_AppLayer()
        {
            // Arrange //
            string myQueueItem = GetProductPriceDispatcher();
            // Deserialize the order data into a dynamic object
            QueueMessageContent payload = JsonConvert.DeserializeObject<QueueMessageContent>(myQueueItem);

            // Deserialize the order data into specified models
            ProductPrices d365_productPrice = JsonConvert.DeserializeObject<ProductPrices>(payload.Body);
            KTI.Moo.Extensions.Cyware.Model.ProductPrice productPrice = new()
            {
                sku_number = d365_productPrice.ItemRelation ?? "",
                upc_code = d365_productPrice.UPCCode ?? "",
                upc_type = d365_productPrice.UPCType ?? "",
                price_event_number = d365_productPrice.PriceJournalNumber ?? "",
                currency_code = d365_productPrice.Currency ?? "",
                price_book = d365_productPrice.DefaultRelationPrice ?? "",
                start_date = d365_productPrice.FromDate ?? "",
                end_date = d365_productPrice.EndDate ?? "",
                promo_flag_yn = d365_productPrice.PromoFlagYN ?? "",
                event_price_multiple = d365_productPrice.EventPriceMultiple ?? "",
                event_price = d365_productPrice.TradeAgreementJournalName ?? "",
                price_method_code = d365_productPrice.PriceMethodCode ?? "",
                mix_match_code = d365_productPrice.MixMatchCode ?? "",
                deal_quantity = d365_productPrice.DealQuantity ?? "",
                deal_price = d365_productPrice.DealPrice ?? "",
                buy_quantity = d365_productPrice.PriceUnit ?? "",
                buy_value = d365_productPrice.AmountInCurrency ?? "",
                buy_value_type = d365_productPrice.BuyValueType ?? "",
                qty_end_value = d365_productPrice.QtyEndValue ?? "",
                quantity_break = d365_productPrice.QuantityBreak ?? "",
                quantity_group_price = d365_productPrice.QuantityGroupPrice ?? "",
                quantity_unit_price = d365_productPrice.AmountInCurrency ?? "",
                cust_promo_code = d365_productPrice.CustomerPromoCode ?? "",
                cust_number = d365_productPrice.CustomerNumber ?? "",
                precedence_level = d365_productPrice.PrecedenceLevel ?? "",
                default_currency = d365_productPrice.DefaultCurrency ?? "",
                default_price_book = d365_productPrice.DefaultPriceBook ?? "",
            };
            string jsonString = JsonConvert.SerializeObject(productPrice);

            // Act //
            bool result = Process(jsonString, productPriceQueueName, connectionString, companyID);

            // Assert //
            Assert.True(result);
        }

        private bool Process(string decodedString, string QueueName, string ConnectionString, string Companyid)
        {
            return dispatcherToQueue.DispatchMessage(decodedString, QueueName, ConnectionString, Companyid);
        }
    }
}
