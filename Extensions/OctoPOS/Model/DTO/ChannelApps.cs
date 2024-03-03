using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.OctoPOS.Model.DTO;

public class ChannelApps
{
    public Model.Invoice invoice { get; set; }

    public Model.Customer customer { get; set; }

    public Model.Order order { get; set; }

    //public Product product { get; set; }
    //public Inventory inventory { get; set; }


}
