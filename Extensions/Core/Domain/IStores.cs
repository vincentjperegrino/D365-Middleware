using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IStores<T> where T : StoreBase
    {
        // basic crud methods
        T Get(int storeId);
        T Get(string storeId);
        T Add(T storeDetails);
        bool Update(T storeDetails);
        bool Delete(int storeId);
        T Upsert(T storeDetails);

    }
}
