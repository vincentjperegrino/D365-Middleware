using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Base.Model;

public class InventoryBase
{

    #region Properties

    public virtual int companyid { get; set; }

    public virtual string domainType { get; init; } = Helpers.DomainType.inventory;

    [StringLength(100)]
    public virtual string bin { get; set; }

    [StringLength(1048576)]
    public virtual string internalflags { get; set; }

    [StringLength(100)]
    public virtual string name { get; set; }
    [Required]
    public virtual string product { get; set; }
    public virtual string sourcesystem { get; set; }
    public virtual string productinventoryid { get; set; }
    [Range(0, 1000000000)]
    public virtual double qtyallocated { get; set; }
    [Range(-1000000000, 1000000000)]
    public virtual  double qtyavailable { get; set; }
    [Range(-1000000000, 1000000000)]
    public virtual double qtyonhand { get; set; }
    [Range(0, 1000000000)]
    public virtual double qtyonorder { get; set; }
    [Range(0, 1000000000)]
    public virtual double reorderpoint { get; set; }
    [StringLength(100)]
    public virtual string row { get; set; }
    [StringLength(100)]
    [Required]
    public virtual string unit { get; set; }
    [Required]
    public virtual string warehouse { get; set; }
    public virtual DateTime createdon { get; set; }
    [Range(0, 1)]
    public virtual int statecode { get; set; }
    [Range(0, 1)]
    public virtual int statuscode { get; set; }
    [JsonIgnore]
    public virtual string moosourcesystem { get; set; }
    [JsonIgnore]
    public virtual string mooexternalid { get; set; }
    public virtual string serialnumber { get; set; }
    public virtual List<string> serialnumbers { get; set; }

    #endregion

}
