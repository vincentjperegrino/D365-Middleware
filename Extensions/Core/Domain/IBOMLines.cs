using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IBOMLines <T> where T : BOMLinesBase
    {
        // basic crud methods
        T Get(int bomID);
        T Get(string bomID);
        T Add(T bomDetails);
        bool Update(T bomDetails);
        bool Delete(int bomID);
        T Upsert(T bomDetails);
    }
}
