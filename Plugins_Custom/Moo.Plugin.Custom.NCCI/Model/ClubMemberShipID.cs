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
    public class ClubMemberShipID : EntityTable
    {
        new public static readonly string EntityName = "kti_clubmembership";
        new public static readonly ColumnSet ColumnSet = new ColumnSet("kti_customer", "kti_clubmembershipautoid");

        public ClubMemberShipID(Entity ClubMemberShipID)
        {
            this.clubmembershipId = ClubMemberShipID.Id;

            if (ClubMemberShipID.Contains("kti_customer"))
            {
                this.kti_customer = ((EntityReference)ClubMemberShipID["kti_customer"]).Id;
            }

            if (ClubMemberShipID.Contains("kti_clubmembershipautoid"))
            {
                this.kti_clubmembershipautoid = (string)ClubMemberShipID["kti_clubmembershipautoid"];
            }
        }

        public Guid clubmembershipId { get; set; }
        public Guid kti_customer { get; set; }
        public string kti_clubmembershipautoid { get; set; }

    }
}
