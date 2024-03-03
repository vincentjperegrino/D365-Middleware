using System;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.SAP.Model;
using KTI.Moo.Operation.Core.Domain.Reports;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.Operation.SAP.App.Dispatcher;

public class DailyReport_Order
{
    private readonly KTI.Moo.Operation.Core.Domain.Reports.IOrder<KTI.Moo.Extensions.SAP.Model.Order, KTI.Moo.CRM.Model.OrderBase> _orderReportService;

    public DailyReport_Order(IOrder<Order, OrderBase> orderReportService)
    {
        _orderReportService = orderReportService;
    }

    [FunctionName("DailyReport_Order_Dispatcher")]
    public void Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
    {

        var MaxNumberOfRetry = 5;

        var retryPolicy = Policy.Handle<System.Exception>().WaitAndRetry(
        MaxNumberOfRetry, // number of retries
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
        (exception, timeSpan, retryCount, context) =>
        {
            log.LogInformation(exception.Message);
        });

        retryPolicy.Execute(() =>
        {

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var PHTtimenow = DateTimeHelper.PHTnow();

            var startdate = DateTimeHelper.PHT_to_UTC(PHTtimenow.AddDays(-1).Date);
            var enddate = DateTime.UtcNow;

            _orderReportService.Process(startdate, enddate, log);

        });

    }
}
