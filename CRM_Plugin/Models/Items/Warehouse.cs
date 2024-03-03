#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Items
{
    /// <summary>
    /// Warehouse
    /// </summary>
    public class Warehouse
    {
        public Warehouse()
        {
        }
        public Warehouse(Warehouse _warehouse)
        {
            #region properties
            this.description = _warehouse.description;
            this.name = _warehouse.name;
            this.warehouseid = _warehouse.warehouseid;
            this.statecode = _warehouse.statecode;
            this.statuscode = _warehouse.statuscode;
            this.mooexternalid = _warehouse.mooexternalid;
            #endregion
        }

        #region Properties
        [StringLength(2000)]
        public string description { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        public string warehouseid { get; set; }
        [Range(0, 1)]
        public int statecode { get; set; }
        [Range(1, 2)]
        public int statuscode { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }
}
