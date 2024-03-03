
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Core.Model
{
    public class InventoryBase
    {
        [JsonIgnore]
        public virtual int companyid { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100)]
        public virtual string bin { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(1048576)]
        public virtual string internalflags { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100)]
        public virtual string name { get; set; }

        [Required]
        public virtual string product { get; set; }

        public virtual string sourcesystem { get; set; }

        public virtual string productinventoryid { get; set; }

        [Range(0, 1000000000)]
        public virtual double qtyallocated { get; set; }

        [Range(-1000000000, 1000000000)]
        public virtual double qtyavailable { get; set; }

        [Range(-1000000000, 1000000000)]
        public virtual double qtyonhand { get; set; }

        [Range(0, 1000000000)]
        public virtual double qtyonorder { get; set; }

        [Range(0, 1000000000)]
        public virtual double reorderpoint { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100)]
        public virtual string row { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100)]
        [Required]
        public virtual string unit { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public virtual string warehouse { get; set; }

        public virtual DateTime createdon { get; set; }

        [Range(0, 1)]
        public virtual int statecode { get; set; }

        [Range(0, 1)]
        public virtual int statuscode { get; set; }

        [DataType(DataType.Text)]
        [JsonIgnore]
        public virtual string moosourcesystem { get; set; }

        [DataType(DataType.Text)]
        [JsonIgnore]
        public virtual string mooexternalid { get; set; }


    }
}
