#region Namespaces
using Microsoft.Xrm.Sdk;
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class Employee
    { 
        public Employee()
        {
        }

        #region Properties
        public EntityReference employeeRecord { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string password { get; set; }
        [DataType(DataType.Text)]
        [StringLength(250)]
        public string passwordFlag { get; set; }
        #endregion
    }
}
