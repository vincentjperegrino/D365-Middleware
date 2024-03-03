using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IDiscountTypeProduct<T> where T : DiscountTypeProductBase
    {
        // basic crud methods
        T Get(int discountTypeCode);
        T Get(string discountTypeCode);
        T Add(T discountTypeDetails);
        bool Update(T discountTypeDetails);
        bool Delete(int discountTypeCode);
        T Upsert(T discountTypeDetails);
    }
}
