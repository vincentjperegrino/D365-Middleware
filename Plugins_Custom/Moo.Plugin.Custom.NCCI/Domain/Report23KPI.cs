using Microsoft.AspNetCore.Mvc;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Domain
{
    public class Report23KPI //: Core.Domain.IReport23KPI
    {
        private readonly IOrganizationService _service;

        private readonly ITracingService _tracingService;


        public Report23KPI(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
        }

        public bool Create(Entity entity)
        {
            throw new NotImplementedException();
        }

        public bool Process(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public bool Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        public bool Upsert(Entity entity)
        {
            throw new NotImplementedException();
        }

        public Entity Get(DateTime YearMonthDateEnd)
        {
            var Query = new QueryExpression();
            Query.EntityName = "kti_monthly23kpi";
            Query.ColumnSet = new ColumnSet(true);

            var entityFilter = new FilterExpression(LogicalOperator.And);
            entityFilter.AddCondition("kti_monthyear", ConditionOperator.Equal, YearMonthDateEnd);

            Query.Criteria.AddFilter(entityFilter);

            var Result = _service.RetrieveMultiple(Query);

            if (Result == null || Result.Entities.Count <= 0)
            {
                return null;
            }

            return Result.Entities.FirstOrDefault();
        }


        public List<Entity> GetCustomers()
        {
            var Query = new QueryExpression();
            Query.EntityName = "contact";
            Query.ColumnSet = new ColumnSet(true);

            var Result = RetrieveAllRecords(_service, Query);

            if (Result == null || Result.Count <= 0)
            {
                return null;
            }

            return Result.ToList();
        }

        private bool GetAllBaseProcess(DateTime DateStart, DateTime DateEnd)
        {

            var Activebasecount = GetActiveBase(DateStart, DateEnd);
            var Inactivebasecount = GetInactivBase(DateStart, DateEnd);
            var Prospectbasecount = GetProspectBase(DateStart, DateEnd);


            return true;
        }


        public decimal? GetAllBase(DateTime DateStart, DateTime DateEnd)
        {
            var Activebasecount = GetActiveBase(DateStart, DateEnd);
            var Inactivebasecount = GetInactivBase(DateStart, DateEnd);
            var Prospectbasecount = GetProspectBase(DateStart, DateEnd);


            var sum = Activebasecount + Inactivebasecount + Prospectbasecount;

            return sum;
        }

        public decimal? GetActiveBase(DateTime DateStart, DateTime DateEnd)
        {
            var fetchXml = $@"<fetch mapping='logical' aggregate='true'>
                                <entity name='contact'>
                                    <attribute name='contactid' aggregate='countcolumn'  alias='customercount' distinct='true'/>
                                         <filter type='and'>                                             
                                             <condition attribute='ncci_lastcoffeepurchasedate' operator='ge' value='{DateStart:yyyy-MM-ddTHH:mm:ss}' />
                                             <condition attribute='ncci_lastcoffeepurchasedate' operator='le' value='{DateEnd:yyyy-MM-ddTHH:mm:ss}' />
                                         </filter>
                                </entity>
                            </fetch>";

            var Result = _service.RetrieveMultiple(new FetchExpression(fetchXml));

            // Get the aggregated customer count
            var distinctCustomersCount = 0;
            if (Result.Entities.Count > 0)
            {
                var customerCount = (AliasedValue)Result.Entities[0]["customercount"];
                distinctCustomersCount = (int)customerCount.Value;
            }

            return distinctCustomersCount;
        }


        public decimal? GetInactivBase(DateTime DateStart, DateTime DateEnd)
        {
            var Customer_ContactType = 714430000;

            var Inactive_Status = 714_430_002;

            string fetchXml = $@"
                     <fetch mapping='logical' aggregate='true'>
                         <entity name='contact'>
                             <attribute name='contactid' aggregate='countcolumn' alias='customercount' />
                             <filter type='and'>
                                 <condition attribute='ncci_lastcoffeepurchasedate' operator='lt' value=' {DateStart:yyyy-MM-ddTHH:mm:ss} ' />
                                 <filter type='or'>
                                     <condition attribute='ncci_customerstatus' operator='eq' value='714430002' />
                                     <condition attribute='ncci_lastcoffeepurchasedate' operator='ge' value='{DateStart:yyyy-MM-ddTHH:mm:ss}' />
                                     <condition attribute='ncci_lastcoffeepurchasedate' operator='le' value='{DateEnd:yyyy-MM-ddTHH:mm:ss}' />
                                 </filter>
                             </filter>
                         </entity>
                     </fetch>";

            var Result = _service.RetrieveMultiple(new FetchExpression(fetchXml));

            // Get the aggregated customer count
            var distinctCustomersCount = 0;
            if (Result.Entities.Count > 0)
            {
                var customerCount = (AliasedValue)Result.Entities[0]["customercount"];
                distinctCustomersCount = (int)customerCount.Value;
            }

            return distinctCustomersCount;
        }

        public decimal? GetProspectBase(DateTime DateStart, DateTime DateEnd)
        {

            var fetchXml = $@"<fetch mapping='logical' aggregate='true'>
                                <entity name='contact'>
                                    <attribute name='contactid' aggregate='countcolumn'  alias='customercount' distinct='true'/>
                                    <link-entity name='salesorder' from='customerid' to='contactid' alias='so' link-type='inner'>                                    
                                    </link-entity>
                                         <filter type='and'>
                                             <condition attribute='ncci_contacttype' operator='eq' value='714430001' />
                                             <condition attribute='createdon' operator='ge' value='{DateStart:yyyy-MM-ddTHH:mm:ss}' />
                                             <condition attribute='createdon' operator='le' value='{DateEnd:yyyy-MM-ddTHH:mm:ss}' />
                                         </filter>
                                </entity>
                            </fetch>";

            var Result = _service.RetrieveMultiple(new FetchExpression(fetchXml));
            // Get the aggregated customer count
            var distinctCustomersCount = 0;
            if (Result.Entities.Count > 0)
            {
                var customerCount = (AliasedValue)Result.Entities[0]["customercount"];
                distinctCustomersCount = (int)customerCount.Value;
            }

            return distinctCustomersCount;
        }


        public Entity GetLastYearSameMonth(DateTime YearMonthDateEnd)
        {
            throw new NotImplementedException();
        }


        public decimal? GetReactivated_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAllBaseMovement_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAllBaseMovement_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageMonthlyCapsules_ActiveBase(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageMonthlyCapsules_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageMonthlyCapsules_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderFrequency_ActiveBase(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderFrequency_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderFrequency_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderSize_ActiveBase(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderSize_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetAverageOrderSize_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivated_ExistingMember_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivated_NewMember_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivated_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivated_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivationRate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivationRate_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetInactivationRate_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }



        public decimal? GetMonthlyCapBuyers_NonPasserby(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders_Channels(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders_NonPasserby(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders_Offline(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders_Online(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCapOrders_Passerby(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCaps_NonPasserby(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCaps_Offline(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCaps_Online(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyCaps_Passerby(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyTotalCaps(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetMonthlyTotalCaps_Channels(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetAcquisition_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetAcquisition_vs_LasYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetAcquisition_YearToDate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetCustomerLossRate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetCustomerLossRate_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetCustomerLossRate_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNetInactive(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewMemberAcquired_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewMemberAcquired_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewMember_Acquired_YearToDate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewPassive_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewProspect_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNewProspect_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetNonPassive(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPasserbyRate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassive(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassiveRate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassiveRate_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassiveRate_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassive_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetPassive_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetProportion_NewMember_ActiveBase(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }



        public decimal? GetReactivated_ExistingMember_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetReactivated_NewMember_vs_LastYear(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetReactivated_vs_LastMonth(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }



        public decimal? GetReactivationRate(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetReactivationRate_ExistingMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
        }

        public decimal? GetReactivationRate_NewMember(DateTime DateStart, DateTime DateEnd)
        {
            throw new NotImplementedException();
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
