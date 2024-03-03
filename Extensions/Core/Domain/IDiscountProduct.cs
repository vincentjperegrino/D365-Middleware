using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IDiscountProduct<T> where T : DiscountProductBase
    {
        // basic crud methods
        T Get(int discountCode);
        T Get(string discountCode);
        T Add(T discountDetails);
        bool Update(T discountDetails);
        bool Delete(int discountCode);
        T Upsert(T discountDetails);
    }
}
