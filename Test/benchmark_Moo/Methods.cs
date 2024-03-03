using System.Linq;

namespace benchmark_Moo
{
    public class Methods
    {
        public static string defaultURL = "http://202.148.162.33:8080/NESPRESSOTEST/api.orm/1.0";

        public string username = "NESPRESSOTEST";
        public string password = "XHTMgtXRTZ";

        public string ApiAuth = "MTAyMTk3MTgwNjE5MDQ1MTM3MDcwNTExMDE5NzQ5MDk2OTA1ODk3NQ==,MTIxNTAxNTMwOTE3MTA5Mzk5MDg5MTY0NTA2Njk2MDcyODMwMjY0NQ==";

        public string redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

        public string companyid = "3388";

        public KTI.Moo.Extensions.OctoPOS.Model.Invoice GetInvoice()
        {
            //KTI.Moo.Extensions.OctoPOS.Domain.Invoice OctoPOSInvoice ;

            ////string InvoiceID = "TJ001S000130";
            //string InvoiceID = "249S00004782g1f111ky";

            //var response = OctoPOSInvoice.Get(InvoiceID);

            return new KTI.Moo.Extensions.OctoPOS.Model.Invoice();
        }



        public MapToThisInvoiceModel mapinvoice(KTI.Moo.Extensions.OctoPOS.Model.Invoice modelfromOctopos)
        {

            var InvoiceModels = new InvoiceModels(modelfromOctopos.InvoiceItems.First());
            var MapInvoiceModels = new MapToThisInvoiceModel(InvoiceModels);

            return MapInvoiceModels;

        }
        public MapToThisInvoiceModel mapinvoicewithdynamic(KTI.Moo.Extensions.OctoPOS.Model.Invoice modelfromOctopos)
        {

            var InvoicewithDynamicModel = new InvoicewithDynamicModel(modelfromOctopos.InvoiceItems.First());
            var MapInvoiceModels = new MapToThisInvoiceModel(InvoicewithDynamicModel);

            return MapInvoiceModels;

        }

    }
}
