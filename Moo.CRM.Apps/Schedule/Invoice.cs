namespace KTI.Moo.CRM.App.Schedule;

public class Invoice
{
    [FunctionName("InvoiceSchedule")]
    public void Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer, ILogger log)
    {
        try
        {
            string Companyid = Environment.GetEnvironmentVariable("CompanyID");

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            CRM.Domain.Invoice domain = new(Convert.ToInt32(Companyid));

            domain.SyncToInvoice().Wait();

        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new Exception(ex.Message);
        }

    }
}
