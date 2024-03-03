using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model.DTO;

public class ChannelApps
{
    public Invoice invoice { get; set; }

    public Customer customer { get; set; } 

    public Order order { get; set; }

    //public Product product { get; set; }
    //public Inventory inventory { get; set; }
}
