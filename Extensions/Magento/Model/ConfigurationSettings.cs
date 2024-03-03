using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ConfigurationSettings<T>
    {
        public T config_value{ get; set; }
        public bool is_config_enable { get; set; }
    }
}
