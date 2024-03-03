using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PluginsUnitTest.CustomPlugin.NCCI
{
    [TestClass]
    public class Report23KPI : TestBase
    {
        private readonly ITracingService _tracingService;

        public Report23KPI()
        {
            _tracingService = Mock.Of<ITracingService>();
        }

        [TestMethod]
        public void GetAllBase()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var domain = new KTI.Moo.Plugin.Custom.NCCI.Domain.Report23KPI(_service, tracingService);

            DateTime startDate = new DateTime(2022, 6, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 6, 30, 23, 59, 59).AddHours(-8);

            var Result = domain.GetAllBase(startDate, endDate);

            Assert.IsTrue(Result > 0);
        }


        [TestMethod]
        public void GetActiveBase()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var domain = new KTI.Moo.Plugin.Custom.NCCI.Domain.Report23KPI(_service, tracingService);

            DateTime startDate = new DateTime(2022, 6, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 6, 30, 23, 59, 59).AddHours(-8);

            var Result = domain.GetActiveBase(startDate, endDate);

            Assert.IsTrue(Result > 0);
        }

        [TestMethod]
        public void GetInactiveBase()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var domain = new KTI.Moo.Plugin.Custom.NCCI.Domain.Report23KPI(_service, tracingService);

            DateTime startDate = new DateTime(2022, 6, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 6, 30, 23, 59, 59).AddHours(-8);

            var Result = domain.GetInactivBase(startDate, endDate);

            Assert.IsTrue(Result > 0);
        }


        [TestMethod]
        public void GetProspect()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var domain = new KTI.Moo.Plugin.Custom.NCCI.Domain.Report23KPI(_service, tracingService);

            DateTime startDate = new DateTime(2022, 6, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 6, 30, 23, 59, 59).AddHours(-8);

            var Result = domain.GetProspectBase(startDate, endDate);

            Assert.IsTrue(Result > 0);
        }

        [TestMethod]
        public void GetOrders()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");

            var domain = new KTI.Moo.Plugin.Custom.NCCI.Domain.Report23KPI(_service, tracingService);

            DateTime startDate = new DateTime(2023, 5, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 5, 31, 23, 59, 59).AddHours(-8);

            var Result = domain.GetCustomers();

            Assert.IsTrue(Result.Count >= 0);
        }

        [TestMethod]
        public void LastMonthvsDateRangeLastMonth()
        {

            _service = connectToCRM("https://nespresso.crm5.dynamics.com");


            List<Entity> Result2 = GetLastMonthWithDateRange();

            List<Entity> Result = GetLastMonth();


            Assert.IsTrue(Result.Count == Result2.Count);
        }

        private List<Entity> GetLastMonthWithDateRange()
        {
            //DateTime today = DateTime.Today;
            //DateTime firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
            //DateTime firstDayOfLastMonth = firstDayOfThisMonth.AddMonths(-1);
            //DateTime lastDayOfLastMonth = firstDayOfThisMonth.AddDays(-1);

            DateTime startDate = new DateTime(2022, 5, 1, 0, 0, 0).AddHours(-8);
            DateTime endDate = new DateTime(2023, 5, 31, 23, 59, 59).AddHours(-8);

            var Query2 = new QueryExpression();
            Query2.EntityName = "salesorder";
            Query2.ColumnSet = new ColumnSet("createdon");

            var entityFilter2 = new FilterExpression(LogicalOperator.And);
            entityFilter2.AddCondition("createdon", ConditionOperator.Between, startDate, endDate);

            Query2.Criteria.AddFilter(entityFilter2);

            var Result2 = RetrieveAllRecords(_service, Query2);
            return Result2;
        }

        private List<Entity> GetLastMonth()
        {
            var Query = new QueryExpression();
            Query.EntityName = "salesorder";
            Query.ColumnSet = new ColumnSet("createdon");


            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("createdon", ConditionOperator.LastMonth);

            Query.Criteria.AddFilter(entityFilter);
            Query.AddOrder("createdon", OrderType.Descending);

            var Result = RetrieveAllRecords(_service, Query);
            return Result;
        }

        [TestMethod]
        public void GetFromKPI()
        {

            _service = connectToCRM("https://nespresso-devt.crm5.dynamics.com");

            DateTime today = DateTime.Today.AddMonths(-2);
            DateTime firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);

            DateTime lastDayOfLastMonth = firstDayOfThisMonth.AddDays(-1).AddHours(-8);

            var Query = new QueryExpression();
            Query.EntityName = "kti_monthly23kpi";
            Query.ColumnSet = new ColumnSet("kti_monthyear");

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_monthyear", ConditionOperator.On, lastDayOfLastMonth.Date);

            Query.Criteria.AddFilter(entityFilter);

            var Result = _service.RetrieveMultiple(Query);


            Assert.IsTrue(Result.Entities.Count > 0);
        }


        private static List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
        {
            var pageNumber = 1;
            var pagingCookie = string.Empty;
            var result = new List<Entity>();
            EntityCollection resp;
            do
            {
                if (pageNumber != 1)
                {
                    query.PageInfo.PageNumber = pageNumber;
                    query.PageInfo.PagingCookie = pagingCookie;
                }
                resp = service.RetrieveMultiple(query);
                if (resp.MoreRecords)
                {
                    pageNumber++;
                    pagingCookie = resp.PagingCookie;
                }
                //Add the result from RetrieveMultiple to the List to be returned.
                result.AddRange(resp.Entities);
            }
            while (resp.MoreRecords);

            return result;
        }


    }
}
