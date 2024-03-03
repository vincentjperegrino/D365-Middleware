using KTI.Moo.Extensions.Lazada.Model;
using KTI.Moo.Extensions.Lazada.Service;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Use in unit test to access internal class
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Distributed;

[assembly: InternalsVisibleTo("TestLazada")]


namespace KTI.Moo.Extensions.Lazada.Domain
{
    internal sealed class Finance
    {
        private Service.ILazopService _service;

        private readonly int MaxPageLimit = 500; // Service.Config.Instance.MaxPaginationInFinanceAPI;

        public Finance(string region, string sellerId)
        {
            this._service = new LazopService(
                Service.Config.Instance.AppKey,
                Service.Config.Instance.AppSecret,
                new Model.SellerRegion() { Region = region, SellerId = sellerId }
            );
        }

        public Finance(string key, string secret, string region, string accessToken)
        {
            this._service = new LazopService(key, secret, region, accessToken, null);
        }

        public Finance(Service.Queue.Config config, IDistributedCache cache)
        {
            this._service = new LazopService(config, cache);
            this.MaxPageLimit = config.MaxPaginationInFinanceAPI;
        }

        public Finance(Service.Queue.Config config, ClientTokens clientTokens)
        {
            this._service = new LazopService(config, clientTokens);

            this.MaxPageLimit = config.MaxPaginationInFinanceAPI;
        }


        public Finance(Service.ILazopService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Get Payout Status of Orders Date.Now as Default
        /// </summary>
        /// <returns></returns>
        public List<PayoutStatus> GetPayoutStatus()
        {
            return GetPayoutStatus(CreatedAfter: DateTime.Now);
        }


        /// <summary>
        /// Get Payout Status of Orders
        /// </summary>
        /// <param name="CreatedAfter">DateTime yyyy-MM-dd </param>
        /// <returns></returns>
        public List<PayoutStatus> GetPayoutStatus(DateTime CreatedAfter)
        {
            string Endpointpath = "/finance/payout/status/get";
            string Method = "GET";

            var parameters = new Dictionary<string, string>
            {
                {"created_after", CreatedAfter.ToString("yyyy-MM-dd")}
            };

            var response = this._service.AuthenticatedApiCall(path: Endpointpath, parameters: parameters, method: Method);
            List<PayoutStatus> returnList = JsonConvert.DeserializeObject<List<PayoutStatus>>(JsonDocument.Parse(response).RootElement.GetProperty("data").ToString());

            return returnList;

        }




        /// <summary>
        /// Get Transaction Details from date range. Lazada limit count always 500.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetails(DateTime StartDate, DateTime EndDate)
        {

            return GetTransactionDetailsMain(StartDate: StartDate
                                       , EndDate: EndDate
                                       , OrderID: 0
                                       , OrderLineID: 0
                                       , TransactionType: 0
                                       , Offset: 0
                                       , Limit: 0);

        }



        public List<TransactionDetail> GetTransactionDetails(long OrderID , DateTime createddate)
        {
            if (OrderID <= 0)
            {
                throw new ArgumentException("Invalid OrderID.", nameof(OrderID));

            }

            var Start = createddate.AddDays(-90);
            var EndDate = createddate.AddDays(90);

            return GetTransactionDetailsMain(StartDate: Start
                                       , EndDate: EndDate
                                       , OrderID: OrderID
                                       , OrderLineID: 0
                                       , TransactionType: 0
                                       , Offset: 0
                                       , Limit: 0);

        }



        /// <summary>
        /// Get Transaction Details from date range with limit and offset. Lazada limit count max is 500.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize">Number of lines of transactions to be extracted. The supported maximum number is 500.</param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetails(DateTime StartDate, DateTime EndDate, int CurrentPage, int PageSize)
        {

            if (CurrentPage < 0)
            {
                throw new ArgumentException("CurrentPage can't be less than 0", nameof(CurrentPage));

            }

            if (PageSize <= 0 || PageSize > MaxPageLimit)
            {
                throw new ArgumentException("Invalid PageSize. Maximum of " + MaxPageLimit, nameof(PageSize));

            }


            int Offset = CurrentPageToOffset(CurrentPage, PageSize);
            int Limit = PageSize;

            return GetTransactionDetailsMain(StartDate: StartDate
                                       , EndDate: EndDate
                                       , OrderID: 0
                                       , OrderLineID: 0
                                       , TransactionType: 0
                                       , Offset: Offset
                                       , Limit: Limit);

        }

        /// <summary>
        /// Get Transaction Details from date range with OrderID, OrderLineID, limit, and offset. Lazada limit count max is 500.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OrderID">Use Order ID from Order API</param>
        /// <param name="OrderLineID">Use Order Line ID from Order API</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize">Number of lines of transactions to be extracted. The supported maximum number is 500.</param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetails(DateTime StartDate, DateTime EndDate, long OrderID, long OrderLineID, int CurrentPage, int PageSize)
        {
            if (OrderID <= 0)
            {
                throw new ArgumentException("Invalid OrderID.", nameof(OrderID));

            }

            if (OrderLineID <= 0)
            {
                throw new ArgumentException("Invalid OrderLineID.", nameof(OrderLineID));

            }


            if (CurrentPage < 0)
            {
                throw new ArgumentException("CurrentPage can't be less than 0", nameof(CurrentPage));

            }

            if (PageSize <= 0 || PageSize > MaxPageLimit)
            {
                throw new ArgumentException("Invalid PageSize. Maximum of " + MaxPageLimit, nameof(PageSize));

            }

            int Offset = CurrentPageToOffset(CurrentPage, PageSize);
            int Limit = PageSize;

            return GetTransactionDetailsMain(StartDate: StartDate
                                       , EndDate: EndDate
                                       , OrderID: OrderID
                                       , OrderLineID: OrderLineID
                                       , TransactionType: 0
                                       , Offset: Offset
                                       , Limit: Limit);

        }

        /// <summary>
        /// Get Transaction Details from date range with OrderID, OrderLineID, TransactionTypeID, limit, and offset. Lazada limit count max is 500.
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="OrderID">Use Order ID from Order API</param>
        /// <param name="OrderLineID">Use Order Line ID from Order API</param>
        /// <param name="TransactionTypeID">Transaction Type ID of Lazada</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize">Number of lines of transactions to be extracted. The supported maximum number is 500.</param>
        /// <returns></returns>
        public List<TransactionDetail> GetTransactionDetails(DateTime StartDate, DateTime EndDate, long OrderID, long OrderLineID, int TransactionTypeID, int CurrentPage , int PageSize)
        {


            if (OrderID <= 0)
            {
                throw new ArgumentException("Invalid OrderID.", nameof(OrderID));

            }

            if (OrderLineID <= 0)
            {
                throw new ArgumentException("Invalid OrderLineID.", nameof(OrderLineID));

            }


            if (CurrentPage < 0)
            {
                throw new ArgumentException("CurrentPage can't be less than 0", nameof(CurrentPage));

            }

            if (PageSize <= 0 || PageSize > MaxPageLimit)
            {
                throw new ArgumentException("Invalid PageSize. Maximum of " + MaxPageLimit , nameof(PageSize));

            }


            if (TransactionTypeID <= -1)
            {
                throw new ArgumentException("Invalid TransactionTypeID", nameof(TransactionTypeID));

            }



            int Offset = CurrentPageToOffset(CurrentPage, PageSize);
            int Limit = PageSize;

            return GetTransactionDetailsMain(StartDate: StartDate
                                       , EndDate: EndDate
                                       , OrderID: OrderID
                                       , OrderLineID: OrderLineID
                                       , TransactionType: TransactionTypeID
                                       , Offset: Offset
                                       , Limit: Limit);

        }


        private List<TransactionDetail> GetTransactionDetailsMain(DateTime StartDate, DateTime EndDate, long OrderID, long OrderLineID, int TransactionType , int Offset , int Limit)
        {
            string Endpointpath = "/finance/transaction/details/get";
            string Method = "GET";

            var parameters = new Dictionary<string, string>();
            parameters.Add("start_time", StartDate.ToString("yyyy-MM-dd"));
            parameters.Add("end_time", EndDate.ToString("yyyy-MM-dd"));

            if (TransactionType > 0)
            {
                parameters.Add("trans_type", TransactionType.ToString());
            }
    
            if (Offset > 0)
            {
                parameters.Add("offset", Offset.ToString());
            }

            if (Limit > 0)
            {
                parameters.Add("limit", Limit.ToString());
            }

            if (OrderID > 0)
            {
                parameters.Add("trade_order_id", OrderID.ToString());
            }

            if (OrderLineID > 0)
            {
                parameters.Add("trade_order_line_id", OrderLineID.ToString());
            }

            var response = this._service.AuthenticatedApiCall(path: Endpointpath, parameters: parameters, method: Method);
            List<TransactionDetail> returnList = JsonConvert.DeserializeObject<List<TransactionDetail>>(JsonDocument.Parse(response).RootElement.GetProperty("data").ToString());

            return returnList;

        }


        private int CurrentPageToOffset(int currentPage , int pagesize)
        {

            if (currentPage < 0)
            {
                throw new ArgumentException("CurrentPage can't be less than 0", nameof(currentPage));

            }

            if (pagesize <= 0 || pagesize > MaxPageLimit)
            {
                throw new ArgumentException("Invalid pagesize. Maximum of " + MaxPageLimit, nameof(pagesize));

            }

            int offset = currentPage * pagesize;

            return offset;

        }






    }
}
