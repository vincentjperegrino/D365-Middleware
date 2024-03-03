using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Model;

public class SearchBase<T> where T : class
{
    public virtual List<T> values { get; set; }
    public virtual int total_count { get; set; }
    public virtual int page_size { get; set; }
    public virtual int current_page { get; set; }
    public virtual int total_pages { get; set; }
}
