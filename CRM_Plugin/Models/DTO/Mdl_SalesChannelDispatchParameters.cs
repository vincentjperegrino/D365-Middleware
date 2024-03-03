#region Namespaces
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
#endregion
using ParameterEntityHelper = CRM_Plugin.EntityHelper;

namespace CRM_Plugin.Models.DTO
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class SalesChannelDTOParameters
    {
        public SalesChannelDTOParameters()
        {
        }

        public static ColumnSet columnSet = new ColumnSet(ParameterEntityHelper.SalesChannelParameter.appKey,
                                                          ParameterEntityHelper.SalesChannelParameter.appSecret,
                                                          ParameterEntityHelper.SalesChannelParameter.password,
                                                          ParameterEntityHelper.SalesChannelParameter.appKeyFlag,
                                                          ParameterEntityHelper.SalesChannelParameter.appSecretFlag,
                                                          ParameterEntityHelper.SalesChannelParameter.passwordFlag);

        public SalesChannelDTOParameters(Entity target)
        {
            try
            {
                #region properties
                this.salesChannel = new EntityReference(target.LogicalName, target.Id);

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.password))
                {
                    this.password = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.password];
                }

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appKey))
                {
                    this.appKey = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.appKey];
                }

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appSecret))
                {
                    this.appSecret = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.appSecret];
                }

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.passwordFlag))
                {
                    this.passwordFlag = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.passwordFlag];
                }

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appKeyFlag))
                {
                    this.appKeyFlag = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.appKeyFlag];
                }

                if (target.Contains(ParameterEntityHelper.SalesChannelParameter.appSecretFlag))
                {
                    this.appSecretFlag = (string)target.Attributes[ParameterEntityHelper.SalesChannelParameter.appSecretFlag];
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        public EntityReference salesChannel { get; set; }
        #region Properties
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string password { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appKey { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appSecret { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string passwordFlag { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appKeyFlag { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string appSecretFlag { get; set; }

        #endregion
    }
}
