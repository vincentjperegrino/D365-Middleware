using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IProducts<T> where T : ProductsBase
    {
        // basic crud methods
        T Get(int productID);
        T Get(string productID);
        T Add(T productDetails);
        bool Update(T productDetails);
        bool Delete(int productID);
        T Upsert(T productDetails, string FileName);
    }
}
