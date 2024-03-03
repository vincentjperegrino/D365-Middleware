using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain
{
    public interface IChannelManagementInventory<T> : IChannelManagement<T> where T : Model.ChannelManagement.SalesChannelBase
    {
    }
}
