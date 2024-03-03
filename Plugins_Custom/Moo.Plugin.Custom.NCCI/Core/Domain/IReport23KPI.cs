using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Core.Domain
{
    public interface IReport23KPI
    {
        bool Process(DateTime DateStart, DateTime DateEnd);

        Entity Get(DateTime YearMonthDateEnd);
        Entity GetLastYearSameMonth(DateTime YearMonthDateEnd);

        bool Update(Entity entity);
        bool Create(Entity entity);
        bool Upsert(Entity entity); 


        decimal? GetAllBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetActiveBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetProspectBase(DateTime DateStart, DateTime DateEnd);

        decimal? GetAllBaseMovement_vs_LastMonth(DateTime DateStart, DateTime DateEnd);
        decimal? GetNewMemberAcquired_vs_LastMonth(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivated_vs_LastMonth(DateTime DateStart, DateTime DateEnd);
        decimal? GetReactivated_vs_LastMonth(DateTime DateStart, DateTime DateEnd);
        decimal? GetNewProspect_vs_LastMonth(DateTime DateStart, DateTime DateEnd);

        decimal? GetAllBaseMovement_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetNewMemberAcquired_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivated_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetReactivated_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetNewProspect_vs_LastYear(DateTime DateStart, DateTime DateEnd);

        decimal? GetInactivated_NewMember_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivated_ExistingMember_vs_LastYear(DateTime DateStart, DateTime DateEnd);

        decimal? GetReactivated_NewMember_vs_LastYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetReactivated_ExistingMember_vs_LastYear(DateTime DateStart, DateTime DateEnd);

        decimal? GetNewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetNonPassive(DateTime DateStart, DateTime DateEnd);
        decimal? GetPassive(DateTime DateStart, DateTime DateEnd);

        decimal? GetPassive_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetPassive_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetNewPassive_vs_LastMonth(DateTime DateStart, DateTime DateEnd);

        decimal? GetAverageMonthlyCapsules_ActiveBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageMonthlyCapsules_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageMonthlyCapsules_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetAverageOrderSize_ActiveBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageOrderSize_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageOrderSize_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetAverageOrderFrequency_ActiveBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageOrderFrequency_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetAverageOrderFrequency_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetProportion_NewMember_ActiveBase(DateTime DateStart, DateTime DateEnd);
        decimal? GetNewMember_Acquired_YearToDate(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetCustomerLossRate(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetCustomerLossRate_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetCustomerLossRate_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetInactivationRate(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivationRate_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetInactivationRate_ExistingMember(DateTime DateStart, DateTime DateEnd);

        decimal? GetReactivationRate(DateTime DateStart, DateTime DateEnd);
        decimal? GetReactivationRate_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetReactivationRate_ExistingMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetInactive(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetAcquisition_vs_LasYear(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetAcquisition_vs_LastMonth(DateTime DateStart, DateTime DateEnd);
        decimal? GetNetAcquisition_YearToDate(DateTime DateStart, DateTime DateEnd);
        decimal? GetPassiveRate(DateTime DateStart, DateTime DateEnd);
        decimal? GetPassiveRate_NewMember(DateTime DateStart, DateTime DateEnd);
        decimal? GetPassiveRate_ExistingMember(DateTime DateStart, DateTime DateEnd);


        decimal? GetMonthlyTotalCaps(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCaps_Passerby(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCaps_NonPasserby(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders_Passerby(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders_NonPasserby(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapBuyers_NonPasserby(DateTime DateStart, DateTime DateEnd);

        decimal? GetMonthlyTotalCaps_Channels(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCaps_Offline(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCaps_Online(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders_Channels(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders_Offline(DateTime DateStart, DateTime DateEnd);
        decimal? GetMonthlyCapOrders_Online(DateTime DateStart, DateTime DateEnd);
        decimal? GetPasserbyRate(DateTime DateStart, DateTime DateEnd);

    }
}
