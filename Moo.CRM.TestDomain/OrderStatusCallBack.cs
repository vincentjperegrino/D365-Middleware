using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain;

public class OrderStatusCallBack
{


    private readonly KTI.Moo.CRM.Domain.Shipment _Domain;
    private readonly ILogger _logger;

    public OrderStatusCallBack()
    {
        _Domain = new(3388);
        _logger = Mock.Of<ILogger>();
    }

    [Fact]
    public async Task GetAllWorking()
    {
        JObject samplejson = JObject.Parse(File.ReadAllText("..\\..\\..\\OrderStatusSample.json"));
        string content = JsonConvert.SerializeObject(samplejson);

        var result = await _Domain.update(content, "b03ac99e-1d70-4dc9-aa3f-b0d16929db33", _logger);

        Assert.IsAssignableFrom<string>(result);
    }


}
