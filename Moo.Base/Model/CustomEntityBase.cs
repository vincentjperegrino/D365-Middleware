using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Model;

public class CustomEntityBase
{
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
