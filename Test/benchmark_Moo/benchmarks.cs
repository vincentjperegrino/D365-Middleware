


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
namespace benchmark_Moo
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class benchmarks
    {
        private static readonly Methods methods = new();

        private KTI.Moo.Extensions.OctoPOS.Model.Invoice Invoices = methods.GetInvoice();


        [Benchmark]
        public void invoice()
        {
            methods.mapinvoice(Invoices);

        }

        [Benchmark]
        public void invoicewithDynamic()
        {
            methods.mapinvoicewithdynamic(Invoices);
        }

    }
}
