using KTI.Moo.Base.Helpers;
using KTI.Moo.CRM.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moo.CRM.TestDomain;

public class Customer
{

    private readonly KTI.Moo.CRM.Domain.Customer _Domain;
    private readonly ILogger _logger;

    public Customer()
    {
        var mock = new Mock<ILogger>();
        _logger = Mock.Of<ILogger>();
        _Domain = new(companyId: 3389);
    }


    [Fact]
    public async Task SearchCustomer()
    {
        var customer = new KTI.Moo.CRM.Model.CustomerBase()
        {
            firstname = "Roxanne Evette",
            lastname = "San Jose",
            emailaddress1 = "xansheepat@gmail.com"
        };

        var result = await _Domain.GetContactBy_Email_MobileNumber_FirstNameLastName(customer);

        Assert.IsType<string>(result);
    }

    [Fact]
    public async Task SearchByMagentoIDCustomer()
    {

        var emailaddress1 = "annajnavarro@gmail.com";
        var mobile = "";
        var magentoid = "";

        var result = await _Domain.GetContactBy_MagentoID_Mobile_Email(magentoid, mobile, emailaddress1);

        Assert.IsType<string>(result);
    }


    [Fact]
    public async Task SearchByMagentoIDCustomerDuplicateDetection_HappyPath()
    {
        var CustomerModel = new CustomerBase();


        CustomerModel.firstname = "Jing Test";
        CustomerModel.lastname = "Opena";

        CustomerModel.emailaddress1 = "Jingtest@jingtest.com";
        CustomerModel.mobilephone = "0998.991103";

        CustomerModel.kti_sourceid = "667103";
        CustomerModel.kti_socialchannelorigin = 959080010;

        var result = await _Domain.GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName(CustomerModel);

        Assert.IsType<List<CustomerBase>>(result);
        Assert.True(result.Count > 0);
    }


    [Fact]
    public async Task SearchByMagentoIDCustomerDuplicateDetection()
    {
        var CustomerModel = new CustomerBase();


        CustomerModel.firstname = "Preston";
        CustomerModel.lastname = "Spradling";

        CustomerModel.emailaddress1 = "prestonspradling@gmail.com";
        CustomerModel.mobilephone = "09293253350".FormatPhoneNumber();

        CustomerModel.kti_sourceid = "648811";
        CustomerModel.kti_socialchannelorigin = KTI.Moo.CRM.Helper.ChannelOrigin.OptionSet_octopos;

        var result = await _Domain.GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName(CustomerModel);

        Assert.IsType<List<CustomerBase>>(result);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public async Task SearchByMagentoIDCustomerDuplicateDetectionWithMagentoID_HappyPath()
    {
        var CustomerModel = new CustomerBase();


        //CustomerModel.firstname = "Jing Test";
        //CustomerModel.lastname = "Opena";

        //CustomerModel.emailaddress1 = "Jingtest@jingtest.com";
        //CustomerModel.mobilephone = "0998.991103";

        //CustomerModel.kti_sourceid = "667103";
        //CustomerModel.kti_socialchannelorigin = 959080010;

        CustomerModel.kti_magentoid = "281991";

        var result = await _Domain.GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName_WithMagentoID(CustomerModel);

        Assert.IsType<List<CustomerBase>>(result);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public void GetAllCustomerList()
    {
        var CustomerList = new List<CustomerBase>();

        var DateNow = DateTime.UtcNow.AddDays(-1);
        var DateEnd = DateTime.UtcNow;

        var result = _Domain.GetAll(CustomerList, DateNow, DateEnd, 10, 1);

        Assert.IsType<List<CustomerBase>>(result);
        Assert.True(result.Count > 0);
    }

    [Fact]
    public void GetAllCustomerList_Default()
    {

        var DateNow = DateTime.UtcNow.AddDays(-1);
        var DateEnd = DateTime.UtcNow;

        var result = _Domain.GetAll(DateNow, DateEnd);

        Assert.IsType<List<CustomerBase>>(result);
        Assert.True(result.Count > 0);
    }


}
