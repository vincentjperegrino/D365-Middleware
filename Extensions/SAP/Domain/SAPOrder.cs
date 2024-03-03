using KTI.Moo.Extensions.SAP.Exception;
using KTI.Moo.Extensions.SAP.Model;
using KTI.Moo.Extensions.SAP.Model.DTO.Orders;
using KTI.Moo.Extensions.SAP.Service;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;

namespace KTI.Moo.Extensions.SAP.Domain;

public class Order : Core.Domain.IOrder<Model.Order, Model.OrderItem>, Core.Domain.ISearch<Model.DTO.Orders.Search, Model.Order>
{

    private readonly ISAPService _service;
    public const string APIDirectory = "/Orders";
    private readonly string _companyid;

    public Order(Config config)
    {
        this._service = new SAPService(config);
        _companyid = config.companyid;
    }

    public Order(string defaultDomain, string redisConnectionString, string username, string password, string companydb)
    {
        this._service = new SAPService(defaultDomain, redisConnectionString, username, password, companydb);
    }

    public Order(ISAPService service)
    {
        this._service = service;
    }

    public Search Get(DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        return GetBySearch(dateFrom, dateTo, pagesize, currentpage);
    }

    public List<Model.Order> GetAll(List<Model.Order> initialList, DateTime dateFrom, DateTime dateTo, int pagesize, int currentpage)
    {
        if (initialList is null)
        {
            initialList = new List<Model.Order>();
        }

        var currentPageSearch = Get(dateFrom, dateTo, pagesize, currentpage);

        if (currentPageSearch is not null && currentPageSearch.values is not null && currentPageSearch.values.Count > 0)
        {
            initialList.AddRange(currentPageSearch.values);

            if (!string.IsNullOrWhiteSpace(currentPageSearch.nextLink))
            {
                GetAll(initialList, dateFrom, dateTo, pagesize, ++currentpage);
            }
        }

        return initialList;
    }

    public List<Model.Order> GetAll(DateTime dateFrom, DateTime dateTo, int pagesize = 20)
    {
        var initialList = new List<Model.Order>();
        return GetAll(initialList, dateFrom, dateTo, pagesize, 1);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <param name="pagesize">Supported only 20</param>
    /// <param name="currentpage"></param>
    /// <returns></returns>
    public Model.DTO.Orders.Search GetBySearch(DateTime dateFrom, DateTime dateTo, int pagesize = 20, int currentpage = 1)
    {
        if (pagesize <= 0)
        {
            throw new System.Exception("Invalid Page size");
        }

        if (currentpage <= 0)
        {
            throw new System.Exception("Invalid Current page");
        }

        //always 20
        pagesize = 20;

        var skip = pagesize * (currentpage - 1);


        string path = $"{APIDirectory}?$filter= (DocDate ge '{dateFrom:yyyy-MM-ddTHH:mm:ss}' and DocDate le '{dateTo:yyyy-MM-ddTHH:mm:ss}') or (UpdateDate ge '{dateFrom:yyyy-MM-ddTHH:mm:ss}' and UpdateDate le '{dateTo:yyyy-MM-ddTHH:mm:ss}') &$skip={skip}";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnDTO = JsonConvert.DeserializeObject<Model.DTO.Orders.Search>(response, settings);


        if (ReturnDTO is null || ReturnDTO.values is null || ReturnDTO.values.Count == 0)
        {
            return new Model.DTO.Orders.Search();
        }

        _ = int.TryParse(_companyid, out var companyid);

        if (companyid != 0 && ReturnDTO.values is not null && ReturnDTO.values.Count > 0)
        {
            ReturnDTO.values = ReturnDTO.values.Select(order =>
            {
                order.companyid = companyid;
                return order;
            }).ToList();

        }

        return ReturnDTO;
    }



    public Model.Order Get(long DocEntry)
    {
        string path = $"{APIDirectory}({DocEntry})";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);


        _ = int.TryParse(_companyid, out var companyid);
        if (companyid != 0)
        {
            ReturnOrderData.companyid = companyid;
        }

        return ReturnOrderData;
    }

    public IEnumerable<OrderItem> GetItems(long DocEntry)
    {
        throw new NotImplementedException();
    }

    public Model.Order Get(string DocEntry)
    {
        string path = $"{APIDirectory}({DocEntry})";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);


        _ = int.TryParse(_companyid, out var companyid);
        if (companyid != 0)
        {
            ReturnOrderData.companyid = companyid;
        }

        return ReturnOrderData;
    }


    /// <summary>
    /// Get by Custom Search
    /// </summary>
    /// <param name="FieldName">Variable name in SAP that will be search. Case sensitive</param>
    /// <param name="FieldValue">Variable value</param>
    /// <returns>Top 1 only</returns>
    public Model.Order GetByField(string FieldName, string FieldValue)
    {
        string path = $"{APIDirectory}?$filter={FieldName} eq '{FieldValue}' and Cancelled eq 'tNO' &$top=1 ";

        bool isAuthenticated = true;
        string method = "GET";

        var response = _service.ApiCall(path, method, isAuthenticated);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        var ReturnDTO = JsonConvert.DeserializeObject<Model.DTO.Orders.Search>(response, settings);


        if (ReturnDTO is null || ReturnDTO.values is null || ReturnDTO.values.Count == 0)
        {
            return new Model.Order();
        }


        var ReturnOrderData = ReturnDTO.values.FirstOrDefault();

        _ = int.TryParse(_companyid, out var companyid);

        if (companyid != 0)
        {
            ReturnOrderData.companyid = companyid;
        }

        return ReturnOrderData;
    }

    public IEnumerable<OrderItem> GetItems(string DocEntry)
    {
        throw new NotImplementedException();
    }


    public Model.Order Add(Model.Order Order)
    {
        var path = APIDirectory;

        var method = "POST";

        var isAuthenticated = true;

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var stringContent = GetContent(Order);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);

        if (ReturnOrderData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPOrder, Method: Add";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnOrderData;
    }

    public Model.Order Update(Model.Order Order)
    {
        var path = $"{APIDirectory}({Order.DocEntry})";

        var method = "PATCH";

        var isAuthenticated = true;

        Order.DocDate = default;
        Order.DocDueDate = default;

        var stringContent = GetContent(Order);

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnOrderData = Get(Order.DocEntry);

        if (ReturnOrderData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPOrder, Method: Update";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnOrderData;
    }


    public Model.Order Upsert(Model.Order Order)
    {
        var ExistingOrder = Get(Order.DocEntry);

        if (ExistingOrder.DocEntry <= 0)
        {
            return Add(Order);
        }

        return Update(Order);

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


    public Model.Order Add(string FromDispatcherQueue)
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

        var ReturnOrderData = JsonConvert.DeserializeObject<Model.Order>(response, settings);

        if (ReturnOrderData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPOrder, Method: Add";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnOrderData;
    }

    public Model.Order Update(string FromDispatcherQueue, string DocEntry)
    {

        var path = $"{APIDirectory}({DocEntry})";

        var method = "PATCH";

        var isAuthenticated = true;

        var stringContent = new StringContent(FromDispatcherQueue, Encoding.UTF8, "application/json");

        var response = _service.ApiCall(path, method, stringContent, isAuthenticated);

        var ReturnOrderData = Get(DocEntry);

        if (ReturnOrderData.DocEntry <= 0)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPOrder, Method: Update";

            throw new SAPIntegrationException(domain, classname, response);
        }

        return ReturnOrderData;
    }


    public Model.Order Upsert(string FromDispatcherQueue)
    {

        var OrderData = JsonConvert.DeserializeObject<JObject>(FromDispatcherQueue);

        var DocEntry = 0;

        if (OrderData.ContainsKey("DocEntry"))
        {
            DocEntry = OrderData["DocEntry"].Value<int>();
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

    public bool CancelOrder(Model.Order Order)
    {
        var path = $"{APIDirectory}({Order.DocEntry})/Cancel";
        var method = "POST";
        var isAuthenticated = true;
        var response = "";

        try
        {
            response = _service.ApiCall(path, method, isAuthenticated);
            return true;

        }
        catch (System.Exception ex)
        {
            var domain = _service.DefaultURL + path;

            var classname = "SAPOrder, Method: CancelOrder";

            throw new SAPIntegrationException(domain, classname, response + " " + ex.ToString());

        }

    }


}
