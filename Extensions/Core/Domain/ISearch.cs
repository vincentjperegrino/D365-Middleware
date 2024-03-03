using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain;

public interface ISearch<T, K> where T : SearchBase<K> where K : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize"></param>
    /// <param name="currentpage">Must be greater than 0</param>
    /// <returns></returns>
    T Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage);

    /// <summary>
    /// Gets all the customers recursively
    /// </summary>
    /// <param name="initialList"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize"></param>
    /// <param name="currentpage">Must be greater than 0</param>
    /// <returns></returns>
    List<K> GetAll(List<K> initialList, DateTime dateFrom, DateTime dateTo, int pagesize , int currentpage);

    /// <summary>
    /// With default pagesize and start in page 1
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize"></param>
    /// <returns></returns>
    List<K> GetAll(DateTime dateFrom, DateTime dateTo , int pagesize = 100);
}
