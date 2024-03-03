

using KTI.Moo.Extensions.SAP.Exception;
using KTI.Moo.Extensions.SAP.Model;
using KTI.Moo.Extensions.SAP.Service;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace KTI.Moo.Extensions.SAP.Domain;

public class Invoice : Core.Domain.IInvoice<Model.Invoice, Model.InvoiceItem>
{

    private readonly ISAPService _service;
    public const string APIDirectory = "/Invoices";
    private readonly string _companyid;

    public Invoice(Config config)
    {
        this._service = new SAPService(config);
        _companyid = config.companyid;
    }

    public Invoice(string defaultDomain, string redisConnectionString, string username, string password, string companydb)
    {
        this._service = new SAPService(defaultDomain, redisConnectionString, username, password, companydb);
    }

    public Invoice(ISAPService service)
    {
        this._service = service;
    }

    public Model.Invoice Get(long DocEntry)
    {
        string path = $"{APIDirectory}({DocEntry})";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);


        _ = int.TryParse(_companyid, out var companyid);
        if (companyid != 0)
        {
            ReturnInvoiceData.companyid = companyid;
        }

        return ReturnInvoiceData;
    }

    public Model.Invoice Get(string DocEntry)
    {
        string path = $"{APIDirectory}({DocEntry})";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);


        _ = int.TryParse(_companyid, out var companyid);
        if (companyid != 0)
        {
            ReturnInvoiceData.companyid = companyid;
        }

        return ReturnInvoiceData;
    }


    public Model.Invoice Add(Model.Invoice Invoice)
    {
        var path = APIDirectory;

        var method = "POST";

        var isAuthenticated = true;

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var stringContent = GetContent(Invoice);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);

        if (ReturnInvoiceData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPInvoice, Method: Add";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnInvoiceData;
    }


    public Model.Invoice Update(Model.Invoice Invoice)
    {
        var path = $"{APIDirectory}({Invoice.DocEntry})";

        var method = "PATCH";

        var isAuthenticated = true;

        Invoice.DocDate = default;
        Invoice.DocDueDate = default;

        var stringContent = GetContent(Invoice);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnInvoiceData = Get(Invoice.DocEntry);

        if (ReturnInvoiceData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPInvoice, Method: Update";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnInvoiceData;
    }



    public Model.Invoice Upsert(Model.Invoice Invoice)
    {
        var Existingcustomer = Get(Invoice.DocEntry);

        if (Existingcustomer.DocEntry <= 0)
        {
            return Add(Invoice);
        }

        return Update(Invoice);

    }


    public IEnumerable<InvoiceItem> GetItemList(long invoiceID)
    {
        throw new NotImplementedException();
    }





    /// <summary>
    /// Get by Custom Search
    /// </summary>
    /// <param name="FieldName">Variable name in SAP that will be search. Case sensitive</param>
    /// <param name="FieldValue">Variable value</param>
    /// <returns>Top 1 only</returns>
    public Model.Invoice GetByField(string FieldName, string FieldValue)
    {
        string path = $"{APIDirectory}?$filter={FieldName} eq '{FieldValue}'&$top=1";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnDTO = JsonConvert.DeserializeObject<Model.DTO.Invoices.Search>(response, settings);


        if (ReturnDTO is null || ReturnDTO.values is null || ReturnDTO.values.Count == 0)
        {
            return new Model.Invoice();
        }


        var ReturnInvoiceData = ReturnDTO.values.FirstOrDefault();

        _ = int.TryParse(_companyid, out var companyid);

        if (companyid != 0)
        {
            ReturnInvoiceData.companyid = companyid;
        }

        return ReturnInvoiceData;
    }


    public Model.Invoice Add(string FromDispatcherQueue)
    {
        var path = APIDirectory;

        var method = "POST";

        var isAuthenticated = true;

        var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var ReturnInvoiceData = JsonConvert.DeserializeObject<Model.Invoice>(response, settings);

        if (ReturnInvoiceData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPInvoice, Method: Add";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnInvoiceData;
    }

    public Model.Invoice Update(string FromDispatcherQueue, string DocEntry)
    {

        var path = $"{APIDirectory}({DocEntry})";

        var method = "PATCH";

        var isAuthenticated = true;

        var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnInvoiceData = Get(DocEntry);

        if (ReturnInvoiceData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPInvoice, Method: Update";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnInvoiceData;
    }


    public Model.Invoice Upsert(string FromDispatcherQueue)
    {

        var invoicedata = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var DocEntry = 0;

        if (invoicedata.ContainsKey("DocEntry"))
        {
            DocEntry = invoicedata["DocEntry"].Value<int>();
        }

        if (DocEntry <= 0)
        {
            return Add(FromDispatcherQueue);
        }

        return Update(FromDispatcherQueue, DocEntry.ToString());

    }

    public bool IsForDispatch(string FromDispatcherQueue)
    {
        return true;
    }

    public bool IsForReceiver(string FromDispatcherQueue)
    {
        return true;
    }



    private static StringContent GetContent(object models)
    {

        var JsonSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(models, Formatting.None, JsonSettings);

        return new StringContent(json, Encoding.UTF8, "application/json");

    }







}
