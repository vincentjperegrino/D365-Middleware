using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IForEx<T> where T : ForExBase
    {
        // basic crud methods
        T Get(int currencyCode);
        T Get(string currencyCode);
        T Add(T forexDetails);
        bool Update(T forexDetails);
        bool Delete(int currencyCode);
        T Upsert(T forexDetails);
    }
}
