using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model;

public class ProductImageBase
{
    public long id { get; set; }
    public long product_id { get; set; }
    public int position { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public string src { get; set; }
    public List<long> variant_ids { get; set; }
}
