
using KTI.Moo.Base.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface ICustomer
{

    Task<string> upsert(string content, ILogger log);
    //Task<string> addbatch(List<T> _customers);
    //Task<string> delete();
    //Task<string> getall();
    //Task<string> get_by_ak(string sourceId, string channelOrigin);
    //Task<string> get_by_id(string _externaluseridentifier, int _sourceChannelOrigin);
    //Task<string> replicate(T _customer);
    //Task<string> replicateall();
    //Task<string> replicatebatch(List<T> _customer);
    //Task<string> replicatebatch(List<string> id);
    //Task<string> replicatebyid(string id);
    //Task<string> update(T _customer);

}
