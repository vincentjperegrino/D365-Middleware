
namespace KTI.Moo.Extensions.Core.Model
{
    public class ImageBase : CreationDateBase
    {
        public virtual bool primary { get; set; }

        public virtual string url { get; set; }
    }
}
