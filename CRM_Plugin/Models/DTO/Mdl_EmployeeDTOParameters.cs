using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.ComponentModel.DataAnnotations;
using ParameterEntityHelper = CRM_Plugin.EntityHelper;

namespace CRM_Plugin.Models.DTO
{
    public class EmployeeDTOParameters
    {
        public EmployeeDTOParameters()
        {
        }

        public static ColumnSet columnSet = new ColumnSet(ParameterEntityHelper.EmployeeParameters.password,
                                                          ParameterEntityHelper.EmployeeParameters.passwordFlag);

        public EmployeeDTOParameters(Entity target)
        {
            try
            {
                #region properties
                this.employeeRecord = new EntityReference(target.LogicalName, target.Id);

                if (target.Contains(ParameterEntityHelper.EmployeeParameters.password))
                {
                    this.password = (string)target.Attributes[ParameterEntityHelper.EmployeeParameters.password];
                }

                if (target.Contains(ParameterEntityHelper.EmployeeParameters.passwordFlag))
                {
                    this.passwordFlag = (string)target.Attributes[ParameterEntityHelper.EmployeeParameters.passwordFlag];
                }

                if (target.Contains(ParameterEntityHelper.EmployeeParameters.salt))
                {
                    this.salt = (string)target.Attributes[ParameterEntityHelper.EmployeeParameters.salt];
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EntityReference employeeRecord { get; set; }
        #region Properties
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string password { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string passwordFlag { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string salt { get; set; }

        #endregion
    }


}
