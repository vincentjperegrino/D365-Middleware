

using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.CRM.Model;

public partial class InvoiceBase : Base.Model.InvoiceBase
{


    #region properties


    //[JsonIgnore]
    //public int coffeeqty { get; set; }
    //[JsonIgnore]
    //public int machineqty { get; set; }
    //[JsonIgnore]
    //public bool coffee { get; set; }
    //[JsonIgnore]
    //public bool noncoffee { get; set; }
    //[JsonIgnore]
    //public bool accessories { get; set; }

    public int kti_socialchannelorigin { get; set; }


    #endregion

}
