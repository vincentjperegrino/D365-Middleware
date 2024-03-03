using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace TestCyware.App.Model;

public class TestBase
{
    public KTI.Moo.Extensions.Cyware.Services.Config config = new()
    {
        BlobStorage = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragerdfuat;AccountKey=CeAuXrjT6+P4cBI2N13oGA+uAI+fCXbvQ3pu5juinilk33QrDcawRn/pfliQIXLG1zB+hsZQuZxP+AStp6kiiA==;EndpointSuffix=core.windows.net",
        AzureConnectionString = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net"
    };
}