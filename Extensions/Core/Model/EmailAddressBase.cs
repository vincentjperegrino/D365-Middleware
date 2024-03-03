
namespace KTI.Moo.Extensions.Core.Model
{
    public class EmailAddressBase : CreationDateBase
    {  

        public virtual bool primary { get; set; }

        public virtual string emailaddress { get; set; }

    }
}
