using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IClientToken_Oauth2<T> where T : Model.ClientTokensBase
    {

        T Create(string authorization);
        T Refresh(T clientToken);
    }
}
