using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IProductCategory<T> where T : ProductCategoryBase
    {
        // basic crud methods
        T Get(int productCategoryId);
        T Get(string productCategoryId);
        T Add(T productCategoryDetails);
        bool Update(T productCategoryDetails);
        bool Delete(int productCategoryId);
        T Upsert(T productCategoryDetails); 
    }
}
