using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using Microsoft.Extensions.Caching.Distributed;

namespace KTI.Moo.Extensions.Lazada.Domain
{
    // TODO create base interface
    public class ReturnRefund
    {
        private Service.ILazopService _service { get; init; }

        public ReturnRefund(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public ReturnRefund(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public ReturnRefund(Service.ILazopService service)
        {
            this._service = service;
        }
        public ReturnRefund(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
        }

        public ReturnRefund(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);   
        }

        public List<ReverseOrderItem> GetItems(int pageSize = 10, int page = 1, long? reverseOrderId = null, long? tradeOrderId = null, IEnumerable<string> ofcStatuses = null, IEnumerable<string> reverseStatusList = null, string returnToType = null, bool? disputeInProgress = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"page_size", pageSize.ToString()},
                {"page_no", page.ToString()}
            };

            if (reverseOrderId.HasValue)
                parameters.Add("reverse_order_id", reverseOrderId.ToString());
            if (tradeOrderId.HasValue)
                parameters.Add("trade_order_id", tradeOrderId.ToString());
            if (ofcStatuses is not null)
                parameters.Add("ofc_status_list", JsonSerializer.Serialize(ofcStatuses));
            if (reverseStatusList is not null)
                parameters.Add("reverse_status_list", JsonSerializer.Serialize(reverseStatusList));
            if (!string.IsNullOrWhiteSpace(returnToType))
                parameters.Add("return_to_type", returnToType);
            if (disputeInProgress is not null)
                parameters.Add("dispute_in_progress", disputeInProgress.ToString());

            var response = this._service.AuthenticatedApiCall("/reverse/getreverseordersforseller", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("result").GetProperty("items");
            return JsonSerializer.Deserialize<List<ReverseOrderItem>>(json.ToString());
        }

        public ReverseOrderDetail GetDetails(long id)
        {
            var parameters = new Dictionary<string, string>
            {
                {"reverse_order_id", id.ToString()}
            };

            var response = this._service.AuthenticatedApiCall("/order/reverse/return/detail/list", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<ReverseOrderDetail>(json.ToString());
        }

        public CancellationEligibility CheckCancellationEligibility(Model.OrderHeader header, IEnumerable<Model.OrderItem> items)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_id", header.kti_sourceid},
                {"order_item_id_list", JsonSerializer.Serialize(items.Select(o => o.kti_sourceid))}
            };

            var response = this._service.AuthenticatedApiCall("/order/reverse/cancel/validate", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<CancellationEligibility>(json.ToString());
        }

        public ReverseOrderCancelTip CancelOrder(long orderId, IEnumerable<long> orderItemIds, string reasonId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"order_id", orderId.ToString()},
                {"order_item_id_list", JsonSerializer.Serialize(orderItemIds)},
                {"reason_id", reasonId}
            };

            var response = this._service.AuthenticatedApiCall("/order/reverse/cancel/create", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<ReverseOrderCancelTip>(json.ToString());
        }

        public List<CancellationReason> GetRejectReasons(long reverseOrderLineId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"reverse_order_line_id", reverseOrderLineId.ToString()}
            };

            var response = this._service.AuthenticatedApiCall("/order/reverse/reason/list", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<List<CancellationReason>>(json.ToString());
        }


        public ReverseOrderUpdate UpdateReverseOrder(string action, long reverseOrderId, IEnumerable<long> reverseOrderItemIds, string reasonId = null, string comment = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"action", action},
                {"reverse_order_id", reverseOrderId.ToString()},
                {"reverse_order_item_ids", JsonSerializer.Serialize(reverseOrderItemIds)}
            };
            if (!string.IsNullOrWhiteSpace(reasonId))
                parameters.Add("reason_id", reasonId);
            if (!string.IsNullOrWhiteSpace(comment))
                parameters.Add("comment", comment);

            var response = this._service.AuthenticatedApiCall("/order/reverse/return/update", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<ReverseOrderUpdate>(json.ToString());
        }

        public ReverseOrderCommunicationHistory GetReverseOrderCommunicationHistory(long reverseOrderLineId, int pageSize = 10, int pageNumber = 1)
        {
            var parameters = new Dictionary<string, string>
            {
                {"reverse_order_line_id", reverseOrderLineId.ToString()},
                {"page_size", pageSize.ToString()},
                {"page_number", pageNumber.ToString()}
            };

            var response = this._service.AuthenticatedApiCall("/order/reverse/return/history/list", parameters, "GET");

            var json = JsonDocument.Parse(response).RootElement.GetProperty("data");
            return JsonSerializer.Deserialize<ReverseOrderCommunicationHistory>(json.ToString());
        }
    }
}
