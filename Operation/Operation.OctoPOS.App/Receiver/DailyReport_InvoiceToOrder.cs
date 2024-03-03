using System;
using KTI.Moo.CRM.Model;
using KTI.Moo.Extensions.Magento.Model;
using KTI.Moo.Extensions.OctoPOS.Helper;
using KTI.Moo.Operation.Core.Domain.Reports;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Polly;

namespace KTI.Moo.Operation.OctoPOS.App.Receiver;

public class DailyReport_InvoiceToOrder
{
    private readonly KTI.Moo.Operation.Core.Domain.Reports.IInvoiceToOrderReport<KTI.Moo.Extensions.OctoPOS.Model.Invoice, KTI.Moo.CRM.Model.OrderBase> _invoiceToOrderReportService;

    public DailyReport_InvoiceToOrder(IInvoiceToOrderReport<Extensions.OctoPOS.Model.Invoice, OrderBase> invoiceToOrderReportService)
    {
        _invoiceToOrderReportService = invoiceToOrderReportService;
    }

    [FunctionName("DailyReport_InvoiceToOrder")]
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

            _invoiceToOrderReportService.Process(startdate, enddate, log);
        });

    }
}
