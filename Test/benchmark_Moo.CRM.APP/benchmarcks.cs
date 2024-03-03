


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace benchmark_Moo.CRM.APP
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class benchmarks
    {
        private static readonly Method methods = new();
        private static readonly string decodedstring = methods.GetJSON();

        [Benchmark]
        public void GetFromJObject()
        {
            methods.GetFromJObject(decodedstring);

        }

        [Benchmark]
        public void GetFromStrongTYPE()
        {
            methods.GetFromStrongTYPE(decodedstring);
        }

    }
}
