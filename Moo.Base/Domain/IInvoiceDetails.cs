using KTI.Moo.Base.Model;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Base.Domain;

public interface IInvoiceDetails
{

    Task<string> upsert(string contents , ILogger log);
    //Task<string> get_by_name_salesman(string _salesmanname);
}

