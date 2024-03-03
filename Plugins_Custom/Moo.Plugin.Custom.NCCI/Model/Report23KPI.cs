using KTI.Moo.Plugin.Custom.NCCI.Core.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Plugin.Custom.NCCI.Model
{
    public class Report23KPI : EntityTable
    {
        new public static readonly string EntityName = "kti_monthly23kpi";
        new public static readonly ColumnSet ColumnSet = new ColumnSet(true);

        public Report23KPI(Entity Reports)
        {

            this.kti_monthly23kpiid = Reports.Id;

            if (Reports.Contains("kti_name"))
            {
                this.kti_name = (string)Reports["kti_name"];
            }

            if (Reports.Contains("kti_monthyear"))
            {
                this.kti_monthyear = (DateTime)Reports["kti_monthyear"];
            }

            if (Reports.Contains("kti_amcactivebase"))
            {
                this.kti_amcactivebase = (decimal)Reports["kti_amcactivebase"];
            }

            if (Reports.Contains("kti_netinactive"))
            {
                this.kti_netinactive = (decimal)Reports["kti_netinactive"];
            }

            if (Reports.Contains("kti_aofactivebaseem"))
            {
                this.kti_aofactivebaseem = (decimal)Reports["kti_aofactivebaseem"];
            }

            if (Reports.Contains("kti_inactivatednmvsly"))
            {
                this.kti_inactivatednmvsly = (decimal)Reports["kti_inactivatednmvsly"];
            }

            if (Reports.Contains("kti_proportionofnminab"))
            {
                this.kti_proportionofnminab = (decimal)Reports["kti_proportionofnminab"];
            }

            if (Reports.Contains("kti_activebasepassive"))
            {
                this.kti_activebasepassive = (decimal)Reports["kti_activebasepassive"];
            }

            if (Reports.Contains("kti_newprospectvsly"))
            {
                this.kti_newprospectvsly = (decimal)Reports["kti_newprospectvsly"];
            }

            if (Reports.Contains("kti_inactivebase"))
            {
                this.kti_inactivebase = (decimal)Reports["kti_inactivebase"];
            }

            if (Reports.Contains("kti_prospectbase"))
            {
                this.kti_prospectbase = (decimal)Reports["kti_prospectbase"];
            }

            if (Reports.Contains("kti_aosactivebasenm"))
            {
                this.kti_aosactivebasenm = (decimal)Reports["kti_aosactivebasenm"];
            }

            if (Reports.Contains("kti_aosactivebaseem"))
            {
                this.kti_aosactivebaseem = (decimal)Reports["kti_aosactivebaseem"];
            }

            if (Reports.Contains("kti_aofactivebasenm"))
            {
                this.kti_aofactivebasenm = (decimal)Reports["kti_aofactivebasenm"];
            }

            if (Reports.Contains("kti_inactivatedvsly"))
            {
                this.kti_inactivatedvsly = (decimal)Reports["kti_inactivatedvsly"];
            }

            if (Reports.Contains("kti_reactivatedemvsly"))
            {
                this.kti_reactivatedemvsly = (decimal)Reports["kti_reactivatedemvsly"];
            }

            if (Reports.Contains("kti_activebasenm"))
            {
                this.kti_activebasenm = (decimal)Reports["kti_activebasenm"];
            }

            if (Reports.Contains("kti_newprospectvslm"))
            {
                this.kti_newprospectvslm = (decimal)Reports["kti_newprospectvslm"];
            }

            if (Reports.Contains("kti_inactivatedvslm"))
            {
                this.kti_inactivatedvslm = (decimal)Reports["kti_inactivatedvslm"];
            }

            if (Reports.Contains("kti_amcactivebaseem"))
            {
                this.kti_amcactivebaseem = (decimal)Reports["kti_amcactivebaseem"];
            }

            if (Reports.Contains("kti_allbase"))
            {
                this.kti_allbase = (decimal)Reports["kti_allbase"];
            }

            if (Reports.Contains("kti_activebasenonpassive"))
            {
                this.kti_activebasenonpassive = (decimal)Reports["kti_activebasenonpassive"];
            }

            if (Reports.Contains("kti_reactivatedvsly"))
            {
                this.kti_reactivatedvsly = (decimal)Reports["kti_reactivatedvsly"];
            }

            if (Reports.Contains("kti_activebaseem"))
            {
                this.kti_activebaseem = (decimal)Reports["kti_activebaseem"];
            }

            if (Reports.Contains("kti_aosactivebase"))
            {
                this.kti_aosactivebase = (decimal)Reports["kti_aosactivebase"];
            }

            if (Reports.Contains("kti_amcactivebasenm"))
            {
                this.kti_amcactivebasenm = (decimal)Reports["kti_amcactivebasenm"];
            }

            if (Reports.Contains("kti_nmacquiredvsly"))
            {
                this.kti_nmacquiredvsly = (decimal)Reports["kti_nmacquiredvsly"];
            }

            if (Reports.Contains("kti_reactivatedvslm"))
            {
                this.kti_reactivatedvslm = (decimal)Reports["kti_reactivatedvslm"];
            }

            if (Reports.Contains("kti_nmacquiredytd"))
            {
                this.kti_nmacquiredytd = (decimal)Reports["kti_nmacquiredytd"];
            }

            if (Reports.Contains("kti_inactivatedemvsly"))
            {
                this.kti_inactivatedemvsly = (decimal)Reports["kti_inactivatedemvsly"];
            }

            if (Reports.Contains("kti_activebasepassiveem"))
            {
                this.kti_activebasepassiveem = (decimal)Reports["kti_activebasepassiveem"];
            }

            if (Reports.Contains("kti_aofactivebase"))
            {
                this.kti_aofactivebase = (decimal)Reports["kti_aofactivebase"];
            }

            if (Reports.Contains("kti_nmacquiredvslm"))
            {
                this.kti_nmacquiredvslm = (decimal)Reports["kti_nmacquiredvslm"];
            }

            if (Reports.Contains("kti_activebasepassivenm"))
            {
                this.kti_activebasepassivenm = (decimal)Reports["kti_activebasepassivenm"];
            }

            if (Reports.Contains("kti_newpassivevslm"))
            {
                this.kti_newpassivevslm = (decimal)Reports["kti_newpassivevslm"];
            }

            if (Reports.Contains("kti_activebase"))
            {
                this.kti_activebase = (decimal)Reports["kti_activebase"];
            }

            if (Reports.Contains("kti_reactivatednmvsly"))
            {
                this.kti_reactivatednmvsly = (decimal)Reports["kti_reactivatednmvsly"];
            }

            if (Reports.Contains("kti_passiverateem"))
            {
                this.kti_passiverateem = (decimal)Reports["kti_passiverateem"];
            }

            if (Reports.Contains("kti_passerbyrate"))
            {
                this.kti_passerbyrate = (decimal)Reports["kti_passerbyrate"];
            }

            if (Reports.Contains("kti_monthlycaporderspasserby"))
            {
                this.kti_monthlycaporderspasserby = (decimal)Reports["kti_monthlycaporderspasserby"];
            }

            if (Reports.Contains("kti_monthlycapordersnonpasserby"))
            {
                this.kti_monthlycapordersnonpasserby = (decimal)Reports["kti_monthlycapordersnonpasserby"];
            }

            if (Reports.Contains("kti_allbasemovementvsly"))
            {
                this.kti_allbasemovementvsly = (decimal)Reports["kti_allbasemovementvsly"];
            }

            if (Reports.Contains("kti_monthlycaporders"))
            {
                this.kti_monthlycaporders = (decimal)Reports["kti_monthlycaporders"];
            }

            if (Reports.Contains("kti_monthlycapordersoffine"))
            {
                this.kti_monthlycapordersoffine = (decimal)Reports["kti_monthlycapordersoffine"];
            }

            if (Reports.Contains("kti_netacquisitionvslm"))
            {
                this.kti_netacquisitionvslm = (decimal)Reports["kti_netacquisitionvslm"];
            }

            if (Reports.Contains("kti_inactivationratenm"))
            {
                this.kti_inactivationratenm = (decimal)Reports["kti_inactivationratenm"];
            }

            if (Reports.Contains("kti_monthlytotalcapschannel"))
            {
                this.kti_monthlytotalcapschannel = (decimal)Reports["kti_monthlytotalcapschannel"];
            }

            if (Reports.Contains("kti_nclrnm"))
            {
                this.kti_nclrnm = (decimal)Reports["kti_nclrnm"];
            }

            if (Reports.Contains("kti_inactivationrateem"))
            {
                this.kti_inactivationrateem = (decimal)Reports["kti_inactivationrateem"];
            }

            if (Reports.Contains("kti_monthlytotalcapsoffline"))
            {
                this.kti_monthlytotalcapsoffline = (decimal)Reports["kti_monthlytotalcapsoffline"];
            }

            if (Reports.Contains("kti_monthlytotalcapsonline"))
            {
                this.kti_monthlytotalcapsonline = (decimal)Reports["kti_monthlytotalcapsonline"];
            }

            if (Reports.Contains("kti_monthlycaporderschannel"))
            {
                this.kti_monthlycaporderschannel = (decimal)Reports["kti_monthlycaporderschannel"];
            }

            if (Reports.Contains("kti_passiverate"))
            {
                this.kti_passiverate = (decimal)Reports["kti_passiverate"];
            }

            if (Reports.Contains("kti_nclrem"))
            {
                this.kti_nclrem = (decimal)Reports["kti_nclrem"];
            }

            if (Reports.Contains("kti_monthlytotalcaps"))
            {
                this.kti_monthlytotalcaps = (decimal)Reports["kti_monthlytotalcaps"];
            }

            if (Reports.Contains("kti_reactivationratenm"))
            {
                this.kti_reactivationratenm = (decimal)Reports["kti_reactivationratenm"];
            }

            if (Reports.Contains("kti_monthlycapbuyersnonpasserby"))
            {
                this.kti_monthlycapbuyersnonpasserby = (decimal)Reports["kti_monthlycapbuyersnonpasserby"];
            }

            if (Reports.Contains("kti_monthlycapordersonline"))
            {
                this.kti_monthlycapordersonline = (decimal)Reports["kti_monthlycapordersonline"];
            }

            if (Reports.Contains("kti_inactivationrate"))
            {
                this.kti_inactivationrate = (decimal)Reports["kti_inactivationrate"];
            }

            if (Reports.Contains("kti_netacquisitionvsly"))
            {
                this.kti_netacquisitionvsly = (decimal)Reports["kti_netacquisitionvsly"];
            }

            if (Reports.Contains("kti_allbasemovementvslm"))
            {
                this.kti_allbasemovementvslm = (decimal)Reports["kti_allbasemovementvslm"];
            }

            if (Reports.Contains("kti_reactivationrateem"))
            {
                this.kti_reactivationrateem = (decimal)Reports["kti_reactivationrateem"];
            }

            if (Reports.Contains("kti_passiveratenm"))
            {
                this.kti_passiveratenm = (decimal)Reports["kti_passiveratenm"];
            }

            if (Reports.Contains("kti_monthlytotalcapspasserby"))
            {
                this.kti_monthlytotalcapspasserby = (decimal)Reports["kti_monthlytotalcapspasserby"];
            }

            if (Reports.Contains("kti_reactivationrate"))
            {
                this.kti_reactivationrate = (decimal)Reports["kti_reactivationrate"];
            }

            if (Reports.Contains("kti_netacquisitionytd"))
            {
                this.kti_netacquisitionytd = (decimal)Reports["kti_netacquisitionytd"];
            }

            if (Reports.Contains("kti_nclr"))
            {
                this.kti_nclr = (decimal)Reports["kti_nclr"];
            }

            if (Reports.Contains("kti_monthlytotalcapsnonpasserby"))
            {
                this.kti_monthlytotalcapsnonpasserby = (decimal)Reports["kti_monthlytotalcapsnonpasserby"];
            }
        }

        public Guid kti_monthly23kpiid { get; set; }
        public string kti_name { get; set; }
        public DateTime kti_monthyear { get; set; }
        public decimal kti_amcactivebase { get; set; }
        public decimal kti_netinactive { get; set; }
        public decimal kti_aofactivebaseem { get; set; }
        public decimal kti_inactivatednmvsly { get; set; }
        public decimal kti_proportionofnminab { get; set; }
        public decimal kti_activebasepassive { get; set; }
        public decimal kti_newprospectvsly { get; set; }
        public decimal kti_inactivebase { get; set; }
        public decimal kti_prospectbase { get; set; }
        public decimal kti_aosactivebasenm { get; set; }
        public decimal kti_aosactivebaseem { get; set; }
        public decimal kti_aofactivebasenm { get; set; }
        public decimal kti_inactivatedvsly { get; set; }
        public decimal kti_reactivatedemvsly { get; set; }
        public decimal kti_activebasenm { get; set; }
        public decimal kti_newprospectvslm { get; set; }
        public decimal kti_inactivatedvslm { get; set; }
        public decimal kti_amcactivebaseem { get; set; }
        public decimal kti_allbase { get; set; }
        public decimal kti_activebasenonpassive { get; set; }
        public decimal kti_reactivatedvsly { get; set; }
        public decimal kti_activebaseem { get; set; }
        public decimal kti_aosactivebase { get; set; }
        public decimal kti_amcactivebasenm { get; set; }
        public decimal kti_nmacquiredvsly { get; set; }
        public decimal kti_reactivatedvslm { get; set; }
        public decimal kti_nmacquiredytd { get; set; }
        public decimal kti_inactivatedemvsly { get; set; }
        public decimal kti_activebasepassiveem { get; set; }
        public decimal kti_aofactivebase { get; set; }
        public decimal kti_nmacquiredvslm { get; set; }
        public decimal kti_activebasepassivenm { get; set; }
        public decimal kti_newpassivevslm { get; set; }
        public decimal kti_activebase { get; set; }
        public decimal kti_reactivatednmvsly { get; set; }
        public decimal kti_passiverateem { get; set; }
        public decimal kti_passerbyrate { get; set; }
        public decimal kti_monthlycaporderspasserby { get; set; }
        public decimal kti_monthlycapordersnonpasserby { get; set; }
        public decimal kti_allbasemovementvsly { get; set; }
        public decimal kti_monthlycaporders { get; set; }
        public decimal kti_monthlycapordersoffine { get; set; }
        public decimal kti_netacquisitionvslm { get; set; }
        public decimal kti_inactivationratenm { get; set; }
        public decimal kti_monthlytotalcapschannel { get; set; }
        public decimal kti_nclrnm { get; set; }
        public decimal kti_inactivationrateem { get; set; }
        public decimal kti_monthlytotalcapsoffline { get; set; }
        public decimal kti_monthlytotalcapsonline { get; set; }
        public decimal kti_monthlycaporderschannel { get; set; }
        public decimal kti_passiverate { get; set; }
        public decimal kti_nclrem { get; set; }
        public decimal kti_monthlytotalcaps { get; set; }
        public decimal kti_reactivationratenm { get; set; }
        public decimal kti_monthlycapbuyersnonpasserby { get; set; }
        public decimal kti_monthlycapordersonline { get; set; }
        public decimal kti_inactivationrate { get; set; }
        public decimal kti_netacquisitionvsly { get; set; }
        public decimal kti_allbasemovementvslm { get; set; }
        public decimal kti_reactivationrateem { get; set; }
        public decimal kti_passiveratenm { get; set; }
        public decimal kti_monthlytotalcapspasserby { get; set; }
        public decimal kti_reactivationrate { get; set; }
        public decimal kti_netacquisitionytd { get; set; }
        public decimal kti_nclr { get; set; }
        public decimal kti_monthlytotalcapsnonpasserby { get; set; }

    }
}
