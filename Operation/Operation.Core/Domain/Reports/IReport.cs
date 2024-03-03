using KTI.Moo.Base.Domain.Queue;
using Microsoft.Extensions.Logging;

namespace KTI.Moo.Operation.Core.Domain.Reports;

public interface IReport<GenericExtensionModel, GenericBaseModel> : INotification
{
    bool Process(DateTime StartDate, DateTime EndDate, ILogger log);

    List<GenericBaseModel> GetListFromCRM(DateTime StartDate, DateTime EndDate);

    List<GenericExtensionModel> GetListFromExtention(DateTime StartDate, DateTime EndDate);

    bool SendToRetryQueue(List<GenericBaseModel> CRM_Model, ILogger log);

    bool SendToRetryQueue(List<GenericExtensionModel> Extension_Model, ILogger log);

}
