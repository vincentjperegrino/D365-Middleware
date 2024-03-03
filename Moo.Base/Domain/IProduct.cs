
using KTI.Moo.Base.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IProduct<T> where T : Base.Model.ProductBase
{
    // basic crud methods
    Task<string> upsert(string contents, ILogger log);


    Task<List<T>> getAll();

}
