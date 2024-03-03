using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IDiscount<T> where T : DiscountBase
    {
        // basic crud methods
        T Get(int discountId);
        T Get(string discountId);
        T Add(T disountHeader);
        bool Update(T discountHeader);
        bool Delete(int discountId);
        T Upsert(T discountHeader);

    }
}
