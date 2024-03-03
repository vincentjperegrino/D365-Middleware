using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface ITender<T> where T : TenderBase
    {
        // basic crud methods
        T Get(int tenderCode);
        T Get(string tenderCode);
        T Add(T tender);
        bool Update(T tender);
        bool Delete(int tenderCode);
        T Upsert(T tender);
    }
}
