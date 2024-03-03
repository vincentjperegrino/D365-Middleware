using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IProductPrice<T> where T : ProductPriceBase
    {
        // basic crud methods
        T Get(int productID);
        T Get(string productID);
        T Add(T productPrice);
        bool Update(T productPrice);
        bool Delete(int productID);
        T Upsert(T productPrice);
    }
}
