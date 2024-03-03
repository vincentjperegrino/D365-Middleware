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
    public class Customer : EntityTable
    {
        new public static readonly string EntityName = "contact";
        new public static readonly ColumnSet ColumnSet = new ColumnSet("ncci_boughtcoffee", "ncci_newclubmembershipid");

        public Customer(Entity Customer)
        {
            this.customerid = Customer.Id;

            if (Customer.Contains("ncci_boughtcoffee"))
            {
                this.ncci_BoughtCoffee = (bool)Customer["ncci_boughtcoffee"];
            }

            if (Customer.Contains("ncci_newclubmembershipid"))
            {
                this.ncci_newclubmembershipid = (string)Customer["ncci_newclubmembershipid"];
            }

        }


        public Guid customerid { get; set; }
        public bool ncci_BoughtCoffee { get; set; }
        public string ncci_newclubmembershipid { get; set; }


    }
}
