#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.UOM
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
}
