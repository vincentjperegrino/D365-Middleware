using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IDiscountLocation<T> where T : DiscountLocationBase
    {
         // basic crud methods
        T Get(int discountLocationId);
        T Get(string discountLocationId);
        T Add(T discountLocation);
        bool Update(T discountLocation);
        bool Delete(int discountLocationId);
        T Upsert(T discountLocation);
    }
}
