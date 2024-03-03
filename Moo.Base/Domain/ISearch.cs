using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain;

public interface ISearch<T, K> where T : Model.SearchBase<K> where K : class
{
    T Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentPage);

    /// <summary>
    /// Gets all the customers recursively
    /// </summary>
    /// <param name="initialList"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    List<K> GetAll(List<K> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage);

    /// <summary>
    /// With default pagesize and start in page 1
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    List<K> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 100);
}