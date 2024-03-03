using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Model;

public class SearchBase<T> where T : class
{
    public virtual List<T> values { get; set; }
    public virtual int total_count { get; set; }
    public virtual int page_size { get; set; }
    public virtual int current_page { get; set; }
}
