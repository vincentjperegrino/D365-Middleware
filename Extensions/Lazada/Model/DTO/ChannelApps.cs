using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model.DTO;

public class ChannelApps<T> where T : Base.Model.ChannelManagement.SalesChannelBase
{

    // public Invoice invoice { get; set; }
    public Customer customer { get; set; }
    public OrderHeader order { get; set; }

    public T config { get; set; }


}
