using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Models.Items
{
    public class Branch : CRM_Plugin.Core.Model.BranchBase
    {
        public Branch()
        {
        }

        public Branch(Entity _entity)
        {
            #region Mapping
            string erroron = "";
            try
            {
                erroron = "kti_branchid";
                this.kti_branchid = !String.IsNullOrEmpty(_entity.Id.ToString()) ? _entity.Id.ToString() : throw new Exception("No GUID for the record");
                erroron = "kti_branchcode";
                this.kti_branchcode = _entity.Contains("kti_branchcode") ? (string)_entity["kti_branchcode"] : default;
                erroron = "kti_name";
                this.kti_name = _entity.Contains("kti_name") ? (string)_entity["kti_name"] : default;
                erroron = "kti_saleschannel";
                this.kti_saleschannel = _entity.Contains("kti_saleschannel") ? (EntityReference)_entity["kti_saleschannel"] : default;
                erroron = "kti_addresscity";
                this.kti_addresscity = _entity.Contains("kti_addresscity") ? (string)_entity["kti_addresscity"] : default;
                erroron = "kti_emailaddress";
                this.kti_emailaddress = _entity.Contains("kti_emailaddress") ? (string)_entity["kti_emailaddress"] : default;
                erroron = "kti_mobilephone";
                this.kti_mobilephone = _entity.Contains("kti_mobilephone") ? (string)_entity["kti_mobilephone"] : default;
                erroron = "kti_phonenumber";
                this.kti_phonenumber = _entity.Contains("kti_phonenumber") ? (string)_entity["kti_phonenumber"] : default;
                erroron = "kti_website";
                this.kti_website = _entity.Contains("kti_website") ? (string)_entity["kti_website"] : default;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + $" {erroron}");
            }
            #endregion
        }

        public Branch(Branch _branch)
        {
            #region properties
            this.kti_branchid = _branch.kti_branchid;
            this.kti_branchcode = _branch.kti_branchcode;
            this.kti_name = _branch.kti_name;
            this.kti_saleschannel = _branch.kti_saleschannel;
            this.kti_addresscity = _branch.kti_addresscity;
            this.kti_emailaddress = _branch.kti_emailaddress;
            this.kti_mobilephone = _branch.kti_mobilephone;
            this.kti_phonenumber = _branch.kti_phonenumber;
            this.kti_website = _branch.kti_website;
            #endregion
        }
    }
}
