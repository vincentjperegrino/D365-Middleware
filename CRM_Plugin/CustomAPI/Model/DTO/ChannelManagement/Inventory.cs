using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement
{
    public class Inventory : Core.Model.ChannelMangement.SalesChannelBase
    {

        public List<CustomField> CustomFieldList { get; set; }
        public List<Model.DTO.Inventory> InventoryList { get; set; }

        public Inventory()
        {

        }

        public Inventory(Entity entity)
        {

            string ErrorEntity = "";

            try
            {
                ErrorEntity = "kti_account";
                if (entity.Contains("kti_account"))
                {
                    this.kti_account = ((EntityReference)entity["kti_account"]).Id.ToString();
                }

                ErrorEntity = "kti_appkey";
                if (entity.Contains("kti_appkey"))
                {
                    this.kti_appkey = (string)entity["kti_appkey"];
                }

                ErrorEntity = "kti_AppKeyflag";
                if (entity.Contains("kti_AppKeyflag"))
                {
                    this.kti_AppKeyflag = (string)entity["kti_AppKeyflag"];
                }

                ErrorEntity = "kti_appsecret";
                if (entity.Contains("kti_appsecret"))
                {
                    this.kti_appsecret = (string)entity["kti_appsecret"];
                }

                ErrorEntity = "kti_AppSecretflag";
                if (entity.Contains("kti_AppSecretflag"))
                {
                    this.kti_AppSecretflag = (string)entity["kti_AppSecretflag"];
                }

                ErrorEntity = "kti_channelorigin";
                if (entity.Contains("kti_channelorigin"))
                {
                    this.kti_channelorigin = ((OptionSetValue)entity["kti_channelorigin"]).Value;
                }

                ErrorEntity = "kti_country";
                if (entity.Contains("kti_country"))
                {
                    this.kti_country = ((EntityReference)entity["kti_country"]).Id.ToString();
                }

                ErrorEntity = "kti_defaulturl";
                if (entity.Contains("kti_defaulturl"))
                {
                    this.kti_defaulturl = (string)entity["kti_defaulturl"];
                }

                ErrorEntity = "kti_isproduction";
                if (entity.Contains("kti_isproduction"))
                {
                    this.kti_isproduction = (bool)entity["kti_isproduction"];
                }

                ErrorEntity = "kti_name";
                if (entity.Contains("kti_name"))
                {
                    this.kti_name = (string)entity["kti_name"];
                }

                ErrorEntity = "kti_databasename";
                if (entity.Contains("kti_databasename"))
                {
                    this.kti_databasename = (string)entity["kti_databasename"];
                }

                ErrorEntity = "kti_password";
                if (entity.Contains("kti_password"))
                {
                    this.kti_password = (string)entity["kti_password"];
                }

                ErrorEntity = "kti_Passwordflag";
                if (entity.Contains("kti_Passwordflag"))
                {
                    this.kti_Passwordflag = (string)entity["kti_Passwordflag"];
                }

                ErrorEntity = "kti_saleschannelcode";
                if (entity.Contains("kti_saleschannelcode"))
                {
                    this.kti_storecode = (string)entity["kti_saleschannelcode"];
                }

                ErrorEntity = "kti_saleschannelId";

                this.kti_saleschannelId = entity.Id.ToString();


                ErrorEntity = "kti_salt";
                if (entity.Contains("kti_salt"))
                {
                    this.kti_salt = (string)entity["kti_salt"];
                }

                ErrorEntity = "kti_username";
                if (entity.Contains("kti_username"))
                {
                    this.kti_username = (string)entity["kti_username"];
                }

                ErrorEntity = "kti_warehousecode";
                if (entity.Contains("kti_warehousecode"))
                {
                    this.kti_warehousecode = (string)entity["kti_warehousecode"];
                }

                ErrorEntity = "kti_sellerid";
                if (entity.Contains("kti_sellerid"))
                {
                    this.kti_sellerid = (string)entity["kti_sellerid"];
                }

                ErrorEntity = "kti_origincompanyid";
                if (entity.Contains("kti_origincompanyid"))
                {
                    this.kti_moocompanyid = (string)entity["kti_origincompanyid"];
                }

                ErrorEntity = "kti_azureconnectionstring";
                if (entity.Contains("kti_azureconnectionstring"))
                {
                    this.kti_azureconnectionstring = (string)entity["kti_azureconnectionstring"];
                }
            }
            catch
            {
                throw new Exception("SalesChannel: Invalid " + ErrorEntity);
            }


        }


    }
}
