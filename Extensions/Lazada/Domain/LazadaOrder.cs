using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using KTI.Moo.Extensions.Core.Domain;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Model.DTO;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;


namespace KTI.Moo.Extensions.Lazada.Domain
{
    public class Order : Core.Domain.IOrder<Model.OrderHeader, Model.OrderItem>, ISearch<Model.DTO.OrderSearch, Model.OrderHeader>
    {
        private Service.ILazopService _service { get; init; }

        private readonly string _sourceURL = "https://sellercenter.lazada.com.ph/order/detail";

        private readonly Service.Queue.Config _config;

        public Order(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public Order(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
            _config = config;

            this._sourceURL = config.BaseSourceUrl;
        }


        public Order(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);
            _config = config;

            this._sourceURL = config.BaseSourceUrl;
        }


        public Order(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public Order(Service.ILazopService service)
        {
            this._service = service;
        }


        public OrderHeader Get(string id)
        {
            if (long.TryParse(id, out var longid))
            {
                return Get(longid);
            }

            return new OrderHeader();
        }

        public Model.OrderHeader Get(long id)
        {
            Model.OrderHeader header = this.GetHeader(id);
            List<Model.OrderItem> items = GetItems(id).ToList();
            var skulist = items.Select(item => item.productid).ToList();
            List<Model.Product> productPriceList = GetProductPriceFromLazada(skulist);
            List<TransactionDetail> transactions = GetTransactionDetailsFromFinance(id, header.laz_CreatedOn);

            var sellerId = $"lazada_{_config.Region}_{_config.SellerId}";
            // header.emailaddress = items[0].laz_DigitalDeliveryInfo;
            header.totallineitemamount = items.Sum(i => i.laz_PaidPrice);
            header.moosourcesystem = sellerId;

            if (!string.IsNullOrWhiteSpace(header.laz_VoucherCode))
            {
                header.laz_VoucherCode = GetPromoCodeFromSellerVoucher(header.laz_VoucherCode);
            }

            header.kti_channelurl = $"{_sourceURL}/{id}/{sellerId}";

            foreach (OrderItem item in items)
            {
                var PriceInOrder = item.priceperunit;
                var originalPrice = productPriceList.Where(productprice => productprice.productid == item.productid).Select(productprice => productprice.price).FirstOrDefault();
                decimal PromoDiscount = 0;

                if (PriceInOrder != originalPrice)
                {
                    item.laz_DailyDiscount = originalPrice - PriceInOrder;
                }

                item.laz_RetailPrice = originalPrice;
                item.item_transaction_details = transactions.Where(transaction => transaction.orderItem_no == item.laz_OrderItemID).ToList();
                item.moosourcesystem = header.moosourcesystem;
                item.shipto_city = header.shipto_city;
                item.shipto_contactname = header.shipto_contactname;
                item.shipto_country = header.shipto_country;
                item.shipto_line1 = header.shipto_line1;
                item.shipto_line2 = header.shipto_line2;
                item.shipto_line3 = header.shipto_line3;
                item.shipto_name = header.shipto_name;
                item.shipto_postalcode = header.shipto_postalcode;
                item.shipto_stateorprovince = header.shipto_stateorprovince;
                item.shipto_telephone = header.shipto_telephone;
                item.priceperunit = originalPrice;
                item.manualdiscountamount = item.laz_VoucherSeller + item.laz_DailyDiscount;
            }

            header.order_items = items;

            return header;
        }

        public Model.OrderHeader GetHeader(long id)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_id", id.ToString()}
            };

            var response = this._service.AuthenticatedApiCall("/order/get", parameters, "GET");
            Model.OrderHeader header = new(JsonDocument.Parse(response).RootElement.GetProperty("data").ToString());

            return header;
        }

        public IEnumerable<Model.OrderItem> GetItems(long orderId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_id", orderId.ToString()}
            };

            var response = this._service.AuthenticatedApiCall("/order/items/get", parameters, "GET");
            List<Model.OrderItem> items = JsonDocument.Parse(response).RootElement.GetProperty("data").EnumerateArray().Select(i => new Model.OrderItem(i.ToString())).ToList();

            if (items is null || items.Count <= 0)
            {
                return new List<Model.OrderItem>();
            }

            //List<TransactionDetail> transactions = GetTransactionDetailsFromFinance(orderId, items.FirstOrDefault().laz_CreatedAt);

            //var seller = $"lazada_{_config.Region}_{_config.SellerId}";
            //var header = this.GetHeader(orderId);
            //foreach (OrderItem item in items)
            //{
            //    item.moosourcesystem = seller;
            //    item.item_transaction_details = transactions.Where(transaction => transaction.orderItem_no == item.laz_OrderItemID).ToList();
            //    item.shipto_city = header.shipto_city;
            //    item.shipto_contactname = header.shipto_contactname;
            //    item.shipto_country = header.shipto_country;
            //    item.shipto_line1 = header.shipto_line1;
            //    item.shipto_line2 = header.shipto_line2;
            //    item.shipto_line3 = header.shipto_line3;
            //    item.shipto_name = header.shipto_name;
            //    item.shipto_postalcode = header.shipto_postalcode;
            //    item.shipto_stateorprovince = header.shipto_stateorprovince;
            //    item.shipto_telephone = header.shipto_telephone;
            //}

            return items;
        }

        public List<Model.OrderItems> GetItems(long[] orderIds)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_ids", System.Text.Json.JsonSerializer.Serialize(orderIds)}
            };

            var response = this._service.AuthenticatedApiCall("/orders/items/get", parameters, "GET");
            List<Model.OrderItems> items = System.Text.Json.JsonSerializer.Deserialize<List<Model.OrderItems>>(JsonDocument.Parse(response).RootElement.GetProperty("data").ToString());

            return items;
        }


        // public List<Models.OrderAndDetails> GetMany(DateTime? updateBefore = null, DateTime? updateAfter = null, DateTime? createdBefore = null, DateTime? createdAfter = null, string sortDirection = "", [Range(0, 4500)] int offset = 0, [Range(0, 100)] int limit = 0, string sortBy = "", string status = "")
        // {
        //     var ordersParameters = new Dictionary<string, string>();

        //     if (updateBefore.HasValue)
        //         ordersParameters.Add("update_before", updateBefore?.ToString("o"));

        //     if (updateAfter.HasValue)
        //         ordersParameters.Add("update_after", updateAfter?.ToString("o"));

        //     if (createdBefore.HasValue)
        //         ordersParameters.Add("created_before", createdBefore?.ToString("o"));

        //     if (createdAfter.HasValue)
        //         ordersParameters.Add("created_after", createdAfter?.ToString("o"));

        //     if (!string.IsNullOrEmpty(sortDirection))
        //         ordersParameters.Add("sort_direction", sortDirection);

        //     if (!string.IsNullOrEmpty(sortBy))
        //         ordersParameters.Add("sort_by", sortBy);

        //     if (offset != 0)
        //         ordersParameters.Add("offset", offset.ToString());

        //     if (limit != 0)
        //         ordersParameters.Add("limit", limit.ToString());

        //     if (!string.IsNullOrEmpty(status))
        //         ordersParameters.Add("status", status);

        //     var response = this._service.AuthenticatedApiCall("/orders/get", ordersParameters, "GET");
        //     List<Models.OrderHeader> headers = JsonSerializer.Deserialize<List<Models.Order>>(JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("orders").ToString());

        //     List<long> orderIds = new();

        //     foreach (Models.OrderHeader order in headers)
        //         orderIds.Add(long.Parse(order.kti_sourceid));

        //     // var orderItems = GetItems(orderIds.ToArray());

        //     // foreach (var (order, index) in orders.Select((o, i) => (o, i)))
        //     //     orders[index].items = orderItems.Single(i => i.OrderId.ToString() == order.kti_sourceid).Items;

        //     return orders;
        // }





        private int CurrentPageToOffset(int currentPage, int pagesize)
        {
            var MaxPageLimit = 100;

            if (currentPage < 1)
            {
                throw new ArgumentException("CurrentPage can't be less than 1", nameof(currentPage));
            }

            if (pagesize <= 0 || pagesize > MaxPageLimit)
            {
                throw new ArgumentException("Invalid pagesize. Maximum of " + MaxPageLimit, nameof(pagesize));

            }

            int offset = currentPage * (pagesize - 1);

            return offset;

        }


        public OrderSearch GetSearchOrderMain(DateTime StartDate, DateTime EndDate, int Offset, int Limit)
        {

            string Endpointpath = "/orders/get";
            string Method = "GET";

            var parameters = new Dictionary<string, string>();
            parameters.Add("created_after", StartDate.ToString("yyyy-MM-ddTHH:mm:ss+08:00"));
            parameters.Add("created_before", EndDate.ToString("yyyy-MM-ddTHH:mm:ss+08:00"));

            if (Offset > 0)
            {
                parameters.Add("offset", Offset.ToString());
            }

            if (Limit > 0)
            {
                parameters.Add("limit", Limit.ToString());
            }

            var response = this._service.AuthenticatedApiCall(path: Endpointpath, parameters: parameters, method: Method);

            var OrdersList = JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("orders").EnumerateArray().Select(i => new Model.OrderHeader(i.ToString())).ToList();

            var Search = new Model.DTO.OrderSearch()
            {
                total_count = JsonDocument.Parse(response).RootElement.GetProperty("data").GetProperty("countTotal").GetInt32(),
                values = OrdersList
            };

            return Search;

        }

        public OrderSearch Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage)
        {
            var Offset = CurrentPageToOffset(currentPage, pagesize);

            return GetSearchOrderMain(dateFrom, dateTo, Offset, pagesize);
        }

        public List<OrderHeader> GetAll(List<OrderHeader> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
        {
            if (initialList is null)
            {
                initialList = new List<OrderHeader>();
            }

            var currentPageSearch = Get(dateFrom, dateTo, pagesize, currentpage);

            if (currentPageSearch is not null && currentPageSearch.values is not null && currentPageSearch.values.Count > 0)
            {
                initialList.AddRange(currentPageSearch.values);

                var currentCount = pagesize * currentpage;

                if (currentPageSearch.total_count > currentCount)
                {
                    GetAll(initialList, dateFrom, dateTo, pagesize, ++currentpage);
                }
            }

            return initialList;
        }

        public List<OrderHeader> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100)
        {
            var initialList = new List<OrderHeader>();
            return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
        }

        public Document GetDocument(List<Model.OrderItem> items, Lazada.Domain.DocumentType type)
        {
            string doctype = type switch
            {
                DocumentType.Invoice => "invoice",
                DocumentType.ShippingLabel => "shippingLabel",
                DocumentType.CarrierManifest => "carrierManifest",
                _ => throw new IndexOutOfRangeException($"Unknown document type '{nameof(type)}.'")
            };

            var parameters = new Dictionary<string, string>
            {
                {"order_item_ids", System.Text.Json.JsonSerializer.Serialize(items.Select(i => i.kti_sourceid))},
                {"doc_type", doctype}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/order/document/get", parameters, "GET")).RootElement.GetProperty("data");
            return System.Text.Json.JsonSerializer.Deserialize<Document>(response.GetProperty("document").ToString());
        }

        public void SetPacked(List<Model.OrderItem> items, string shipmentProvider, string deliveryType)
        {
            var parameters = new Dictionary<string, string>
            {
                {"shipping_provider", shipmentProvider},
                {"delivery_type", deliveryType},
                {"order_item_ids", System.Text.Json.JsonSerializer.Serialize(items.Select(o => o.kti_sourceid))}
            };

            var response = this._service.AuthenticatedApiCall("/order/pack", parameters, "POST");
            var packedOrders = System.Text.Json.JsonSerializer.Deserialize<List<PackedOrder>>(JsonDocument.Parse(response).RootElement.GetProperty("data").ToString());
            foreach (PackedOrder packedOrder in packedOrders)
            {
                var index = items.FindIndex(i => i.kti_sourceid == packedOrder.order_item_id.ToString());
                if (index != -1)
                {
                    items[index].laz_ShipmentProvider = packedOrder.shipment_provider;
                    items[index].laz_PackageId = packedOrder.package_id;
                    items[index].laz_TrackingCode = packedOrder.tracking_number;
                    items[index].laz_PurchaseOrderId = packedOrder.purchase_order_id;
                    items[index].laz_PurchaseOrderNumber = packedOrder.purchase_order_number;
                }
            }
        }

        public void Repack(Model.OrderItem item)
        {
            var parameters = new Dictionary<string, string>
            {
                {"package_id", item.laz_PackageId}
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/order/repack", parameters, "POST")).RootElement.GetProperty("data");
            item.laz_PackageId = response.GetProperty("package_id").ToString();
        }

        public void SetReadyToShip(IEnumerable<Model.OrderItem> items, string deliveryType = "dropship", string shipmentProvider = null, string trackingCode = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_item_ids", System.Text.Json.JsonSerializer.Serialize(items.Select(i => i.kti_sourceid))},
                {"delivery_type", deliveryType},
                {"shipment_provider", string.IsNullOrWhiteSpace(shipmentProvider) ? items.First().laz_ShipmentProvider : shipmentProvider},
                {"tracking_number", string.IsNullOrWhiteSpace(trackingCode) ? items.First().laz_TrackingCode : trackingCode}
            };

            this._service.AuthenticatedApiCall("/order/rts", parameters, "POST");
        }

        public void SetDelivered(IEnumerable<Model.OrderItem> items)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_item_ids", System.Text.Json.JsonSerializer.Serialize(items.ToList().Select(i => i.kti_sourceid))}
            };

            this._service.AuthenticatedApiCall("/order/sof/delivered", parameters, "POST");
        }

        public void SetDeliveryFailed(List<Model.OrderItem> items)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_item_ids", System.Text.Json.JsonSerializer.Serialize(items.Select(i => i.kti_sourceid))}
            };
        }

        public void SetCancelled(string itemid, CancelReason reasonId, string reasonDetail = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_item_id", itemid},
                {"reason_id", ((int)reasonId).ToString()}
            };

            if (!string.IsNullOrWhiteSpace(reasonDetail))
                parameters.Add("reason_detail", reasonDetail);

            var response = this._service.AuthenticatedApiCall("/order/cancel", parameters, "POST");
            //if (JsonDocument.Parse(response).RootElement.GetProperty("success").Equals("true"))
            //{
            //    item.laz_ReasonDetail = reasonDetail;
            //    item.laz_Reason = "Cancel";
            //}
        }
        public static int ConvertStatus(IEnumerable<string> lazadaStatuses)
        {
            if (lazadaStatuses.Count() == 0)
            {
                return 959080000;
            }


            string[] pending = { "unpaid", "pending", "packed", "repacked", "ready_to_ship_pending", "ready_to_ship", "shipped", "shipped_back", "shipped_back_success", "shipped_back_failed", "failed_delivery", "lost_by_3pl", "destroyed_by_3pl" };
            string[] successful = { "delivered" };
            string[] returned = { "returned" };
            string[] cancelled = { "canceled", "cancelled" };

            if (lazadaStatuses.Intersect(pending).Any())
            {
                return 959080000;
            }

            if (lazadaStatuses.Intersect(successful).Any())
            {
                return 959080001;
            }

            if (lazadaStatuses.Intersect(returned).Any())
            {
                return 959080010;
            }

            if (lazadaStatuses.Intersect(cancelled).Any())
            {
                return 959080007;
            }


            return 959080000;
        }

        public bool SetInvoiceNumber(string order_item_id, string invoice_id)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_item_id", order_item_id},
                {"invoice_number", invoice_id},
            };

            var response = JsonDocument.Parse(this._service.AuthenticatedApiCall("/order/invoice_number/set", parameters, "POST")).RootElement.GetProperty("data");

            if (response.GetProperty("invoice_number").ToString() == invoice_id && response.GetProperty("order_item_id").ToString() == order_item_id)
            {
                return true;
            }

            return false;

        }


        public List<TransactionDetail> GetTransactionDetailsFromFinance(long orderID, DateTime createddate)
        {
            Finance LazadaFinance = new(_service);

            return LazadaFinance.GetTransactionDetails(orderID, createddate);
        }



        public List<Model.Product> GetProductPriceFromLazada(List<string> skus)
        {
            Domain.Product LazadaProduct = new(_service);

            return LazadaProduct.GetProductPrice(skus);
        }


        public string GetPromoCodeFromSellerVoucher(string voucherid)
        {
            SellerVoucher SellerVoucher = new(_service);

            var promodetails = SellerVoucher.GetFromID(voucherid);

            return string.IsNullOrWhiteSpace(promodetails.kti_promocode) ? voucherid : promodetails.kti_promocode;
        }



        public OrderHeader Add(OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Update(OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Upsert(OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Add(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Update(string FromDispatcherQueue, string Orderid)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Upsert(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public OrderHeader GetByField(string FieldName, string FieldValue)
        {
            throw new NotImplementedException();
        }

        public bool IsForDispatch(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public bool IsForReceiver(string FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }

        public bool CancelOrder(OrderHeader FromDispatcherQueue)
        {
            throw new NotImplementedException();
        }


    }

    public record Document
    {
        public string File { get; set; }
        [JsonPropertyName("mime_type")]
        public string Mimetype { get; set; }
        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; }
    }

    public enum DocumentType
    {
        Invoice,
        ShippingLabel,
        CarrierManifest
    }

    public record PackedOrder
    {
        public int order_item_id { get; set; }
        public long purchase_order_id { get; set; }
        public long purchase_order_number { get; set; }
        public string tracking_number { get; set; }
        public string shipment_provider { get; set; }
        public string package_id { get; set; }
    }
}