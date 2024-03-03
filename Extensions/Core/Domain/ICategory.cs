using System.Collections.Generic;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface ICategory<T> where T : Core.Model.CategoryBase
    {
        List<T> Get();

        T Add(T categoryModel);

        T Get(int categoryID);

        T Update(T categoryModel);

        bool Delete(int categoryID);

    }
}