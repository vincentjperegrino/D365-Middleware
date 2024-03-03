#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.UOM
{
    /// <summary>
    /// Unit of measurement
    /// </summary>
    public class UnitOfMeasurement
    {
        public UnitOfMeasurement(UnitOfMeasurement _unitOfMeasurement)
        {
            #region properties
            this.baseuom = _unitOfMeasurement.baseuom;
            this.name = _unitOfMeasurement.name;
            this.quantity = _unitOfMeasurement.quantity;
            this.uomscheduleid = _unitOfMeasurement.uomscheduleid;
            this.mooexternalid = _unitOfMeasurement.mooexternalid;
            this.moosourcesystem = _unitOfMeasurement.moosourcesystem;
            #endregion
        }

        public UnitOfMeasurement()
        {
        }

        #region Properties
        [Required]
        public string baseuom { get; set; }
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        [Range(0, 10000000000)]
        public decimal quantity { get; set; }
        public string uomid { get; set; }
        public string uomscheduleid { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }
}
