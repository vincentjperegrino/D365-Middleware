using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class PromoBase
{
    // public string kti_saleschannel { get; set; }
    public virtual int companyid { get; set; }
    public virtual string kti_name { get; set; }
    public virtual string kti_promoid { get; set; }
    public virtual string kti_channelid { get; set; }
    public virtual string kti_promocode { get; set; }
    public virtual decimal kti_discountamount { get; set; }
    public virtual decimal kti_discountpercent { get; set; }
    public virtual bool kti_freedelivery { get; set; }
    public virtual bool kti_ismaxdiscountamount { get; set; }
    public virtual decimal kti_maxdiscountamount { get; set; }
    public virtual decimal kti_minspent { get; set; }
    public virtual decimal kti_minquantity { get; set; }

    public virtual DateTime kti_validfrom { get; set; }
    public virtual DateTime kti_validto { get; set; }



    //default fields
    public virtual string createdby { get; set; }
    public virtual DateTime createdon { get; set; }
    public virtual string createdonbehalfby { get; set; }
    public virtual int importsequencenumber { get; set; }
    public virtual string modifiedby { get; set; }
    public virtual DateTime modifiedon { get; set; }
    public virtual string modifiedonbehalfby { get; set; }
    public virtual DateTime overriddencreatedon { get; set; }
    public virtual string ownerid { get; set; }
    public virtual string owningbusinessunit { get; set; }
    public virtual string owningteam { get; set; }  
    public virtual string owninguser { get; set; }
    public virtual int statecode { get; set; }
    public virtual int statuscode { get; set; }

}
