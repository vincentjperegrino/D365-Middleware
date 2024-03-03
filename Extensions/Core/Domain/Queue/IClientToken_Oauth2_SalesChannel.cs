using KTI.Moo.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain.Queue
{
    public interface IClientToken_Oauth2_SalesChannel<T, R> : IClientToken_Oauth2<T>, IChannelManagement<R> where T : Model.ClientTokensBase where R : Base.Model.ChannelManagement.SalesChannelBase
    {
        bool UpdateToken(T Token, R ChannelConfig);
        T Refresh(T Token, R ChannelConfig);

    }
}
