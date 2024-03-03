#region Namespaces
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
#endregion

namespace CRM_Plugin.Models.Customer
{

    /// <summary>
    /// Customer
    /// </summary>
    public class Account
    {
        public string EntityName = "account";
        public string PrimaryKey = "accountid";

        public Account()
        {
        }

        public Account(Entity _entity)
        {
            #region Mapping
            string erroron = "";

            try
            {
                erroron = "accountid";
                this.accountid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");
            }
            #endregion
        }

        public Account(Entity _entity, IOrganizationService service)
        {
            #region properties
            string erroron = "";
            try
            {
                erroron = "accountid";
                this.accountid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");

            }



            #endregion
        }

        public Account(Account _customer)
        {
            #region Mapping
            this.accountid = _customer.accountid;
            #endregion
        }

        #region properties
        public string accountid { get; set; }

        #endregion
    }
}
