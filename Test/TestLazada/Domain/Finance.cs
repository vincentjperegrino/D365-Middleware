using KTI.Moo.Extensions.Lazada.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace TestLazada.Domain
{
    public class Finance : Base.LazadaBase
    {


        private readonly KTI.Moo.Extensions.Lazada.Domain.Finance FinanceDomain;
        private readonly IDistributedCache _cache;

        public Finance()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisconnection; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

            FinanceDomain = new(CongfigTest, _cache);
        }

        [Fact]
        public void TestGetPayoutStatusWithParametersIFworking()
        {

         
            DateTime SelectedTime = DateTime.Parse("2018-01-01");

            var response = FinanceDomain.GetPayoutStatus(SelectedTime);


            Assert.IsAssignableFrom<List<PayoutStatus>>(response);


        }

        [Fact]
        public void TestGetPayoutStatusIFworking()
        {

            var response = FinanceDomain.GetPayoutStatus();

            Assert.IsAssignableFrom<List<PayoutStatus>>(response);

        }


        [Fact]
        public void TestGetTransactionDetailsDateRangeOnlyIFworking()
        {

            DateTime StartDate = DateTime.Parse("2018-01-01");
            DateTime EndDate = DateTime.Parse("2021-01-05");


            var response = FinanceDomain.GetTransactionDetails(StartDate:StartDate,EndDate);

            Assert.IsAssignableFrom<List<TransactionDetail>>(response);

        }


        [Fact]
        public void TestGetTransactionDetailsDateRangeWithOffsetAndLimitIFworking()
        {
            DateTime StartDate = DateTime.Parse("2018-01-01");
            DateTime EndDate = DateTime.Parse("2021-01-05");
            int Offset = 0;
            int Limit = 100;


            var response = FinanceDomain.GetTransactionDetails(StartDate: StartDate, EndDate, Offset , Limit);

            Assert.IsAssignableFrom<List<TransactionDetail>>(response);
            Assert.Equal(Limit, response.Count);
        }


        [Fact]
        public void TestGetTransactionDetailsDateRangeWith_Offset_Limit_OrderID_OrderLineID_IFworking()
        {


            DateTime StartDate = DateTime.Parse("2018-01-01");
            DateTime EndDate = DateTime.Parse("2021-01-05");
            int Offset = 0;
            int Limit = 100;
            long OrderId = 276350300150728;
            long OrderLineID = 276350300350728;


            var response = FinanceDomain.GetTransactionDetails(StartDate: StartDate, EndDate , OrderId , OrderLineID, Offset, Limit);

            Assert.IsAssignableFrom<List<TransactionDetail>>(response);
        }

        [Fact]
        public void TestGetTransactionDetailsDateRangeWith_Offset_Limit_OrderID_OrderLineID_TransactionTypeID_IFworking()
        {       

            DateTime StartDate = DateTime.Parse("2018-01-01");
            DateTime EndDate = DateTime.Parse("2021-01-05");
            int PageNumber = 1;
            int PageSize = 20;
            long OrderId = 276350300150728;
            long OrderLineID = 276350300350728;
            int TransactionTypeID = -1;

            var response = FinanceDomain.GetTransactionDetails(StartDate: StartDate, EndDate, OrderId, OrderLineID,TransactionTypeID, PageNumber, PageSize);

            Assert.IsAssignableFrom<List<TransactionDetail>>(response);
        }


        [Fact]
        public void TestGetTransactionDetailsOrderID_IFworking()
        {
            long OrderId = 276350300150728;
            var SelectedDate = DateTime.Now;

            var response = FinanceDomain.GetTransactionDetails(OrderId , SelectedDate);

            Assert.IsAssignableFrom<List<TransactionDetail>>(response);
        }

    }
}
