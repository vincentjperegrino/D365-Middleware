#region Namespaces
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace CRM_Plugin.Models.Items
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(Inventory _inventory)
        {
            #region properties
            this.bin = _inventory.bin;
            this.internalflags = _inventory.internalflags;
            this.name = _inventory.name;
            this.product = _inventory.product;
            this.productinventoryid = _inventory.productinventoryid;
            this.qtyallocated = _inventory.qtyallocated;
            this.qtyavailable = _inventory.qtyavailable;
            this.qtyonhand = _inventory.qtyonhand;
            this.qtyonorder = _inventory.qtyonorder;
            this.reorderpoint = _inventory.reorderpoint;
            this.row = _inventory.row;
            this.unit = _inventory.unit;
            this.warehouse = _inventory.warehouse;
            this.createdon = _inventory.createdon;
            this.statecode = _inventory.statecode;
            this.statuscode = _inventory.statuscode;
            this.mooexternalid = _inventory.mooexternalid;
            this.moosourcesystem = _inventory.moosourcesystem;
            #endregion
        }

        #region Properties
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string bin { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(1048576)]
        public string internalflags { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        public string product { get; set; }
        public string sourcesystem { get; set; }
        public string productinventoryid { get; set; }
        [Range(0, 1000000000)]
        public double qtyallocated { get; set; }
        [Range(-1000000000, 1000000000)]
        public double qtyavailable { get; set; }
        [Range(-1000000000, 1000000000)]
        public double qtyonhand { get; set; }
        [Range(0, 1000000000)]
        public double qtyonorder { get; set; }
        [Range(0, 1000000000)]
        public double reorderpoint { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string row { get; set; }
        [DataType(DataType.Text)]
        [StringLength(100)]
        [Required]
        public string unit { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string warehouse { get; set; }
        public DateTime createdon { get; set; }
        [Range(0, 1)]
        public int statecode { get; set; }
        [Range(0, 1)]
        public int statuscode { get; set; }
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        #endregion
    }
}
