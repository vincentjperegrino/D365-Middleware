#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM.Models.UOM
{

    /// <summary>
    /// Unit group
    /// </summary>
    public class UnitGroup
    {

        public UnitGroup()
        {
        }

        public UnitGroup(UnitGroup _unitGroup)
        {
            #region properties
            this.baseuomname = _unitGroup.baseuomname;
            this.description = _unitGroup.description;
            this.name = _unitGroup.name;
            this.mooexternalid = _unitGroup.mooexternalid;
            this.moosourcesystem = _unitGroup.moosourcesystem;
            #endregion
        }

        #region Properties
        [StringLength(100)]
        public string baseuomname { get; set; }
        [StringLength(2000)]
        public string description { get; set; }
        [Required]
        [StringLength(200)]
        public string name { get; set; }
        public int statecode { get; set; }
        public int statuscode { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }

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

    /// <summary>
    /// UOM
    /// </summary>
    public class UOM
    {
    }
}
