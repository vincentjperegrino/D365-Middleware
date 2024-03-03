using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain;

public interface IChannelManagement<T> where T : Model.ChannelManagement.SalesChannelBase
{
    public int CompanyID { get; set; }
    T Get(string StoreCode);
    T GetbyLazadaSellerID(string SellerID);
    bool UpdateToken(T ChannelConfig);

    List<T> GetChannelList();

}
