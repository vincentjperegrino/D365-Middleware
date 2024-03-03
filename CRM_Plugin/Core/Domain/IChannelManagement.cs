using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Domain
{
    public interface IChannelManagement<T> where T : Model.ChannelMangement.SalesChannelBase
    {

        T Get(string ChannelCode);
        T GetbyLazadaSellerID(string SellerID);
        bool UpdateToken(T ChannelConfig);
        List<T> GetChannelList();

    }
}
