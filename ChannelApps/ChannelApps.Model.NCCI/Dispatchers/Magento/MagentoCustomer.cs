using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.Magento;

public class Customer : ICustomerToQueue
{
    private readonly IDistributedCache _cache;

    public Customer()
    {

    }

    public Customer(IDistributedCache cache)
    {
        _cache = cache;
    }

    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {
        var configFromQueueObject = JsonConvert.DeserializeObject<JObject>(messagequeue);

        var configFromQueueSalesChannelObject = configFromQueueObject[KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname];

        var saleschannel = JsonConvert.SerializeObject(configFromQueueSalesChannelObject);

        var configFromQueueSalesChannel = JsonConvert.DeserializeObject<CRM.Model.ChannelManagement.SalesChannel>(saleschannel);

        KTI.Moo.Extensions.Magento.Service.Config Config = new()
        {
            defaultURL = configFromQueueSalesChannel.kti_defaulturl,
            redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False",
            password = configFromQueueSalesChannel.kti_password,
            username = configFromQueueSalesChannel.kti_username,
            companyid = CompanyID,

        };

        var customer = JsonConvert.DeserializeObject<Domain.Models.Customer.Customer>(messagequeue);

        KTI.Moo.Extensions.Magento.Model.Customer MagentoCustomer = new();

        MagentoCustomer.customer_id = GetCustomerId(Config, customer);

        //MagentoCustomer.email = customer.emailaddress1;
        //MagentoCustomer.firstname = customer.firstname;
        //MagentoCustomer.lastname = customer.lastname;

        MagentoCustomer.taxvat = string.IsNullOrWhiteSpace(customer.ncci_clubmembershipid) ? string.Empty : customer.ncci_clubmembershipid;

        //MagentoCustomer.address = new();

        //MagentoCustomer.address.Add(GetAddress1FromCrm(Config, customer));

        //if (!string.IsNullOrWhiteSpace(customer.address2_line1))
        //{
        //    MagentoCustomer.address.Add(GetAddress2FromCrm(Config, customer));
        //}

        //if (!string.IsNullOrWhiteSpace(customer.address3_line1))
        //{
        //    MagentoCustomer.address.Add(GetAddress3FromCrm(Config, customer));
        //}


        return SendMessageToQueue(MagentoCustomer, QueueName, QueueConnectionString);
    }

    private static bool SendMessageToQueue(object clientModel, string QueueName, string ConnectionString)
    {
        var Json = GetJsonForMessageQueue(clientModel);

        QueueClient queueClient = new QueueClient(ConnectionString, QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        });

        queueClient.CreateIfNotExists();
        queueClient.SendMessage(Json);

        return true;
    }

    private static string GetJsonForMessageQueue(object clientModel)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);
        return Json;
    }


    private int GetCustomerId(KTI.Moo.Extensions.Magento.Service.Config config, Domain.Models.Customer.Customer customer)
    {
   
        if (customer.kti_socialchannelorigin == 959080010)//is Magento channel origin
        {
            var isValidCustomerID = int.TryParse(customer.kti_sourceid, out var customerid);

            if (isValidCustomerID)
            {
                return customerid;
            }

        }

        if (string.IsNullOrWhiteSpace(customer.kti_magentoid))
        {
            var isValidCustomerID = int.TryParse(customer.kti_magentoid, out var customerid);

            if (isValidCustomerID)
            {
                return customerid;
            }

        }

        KTI.Moo.Extensions.Magento.Domain.Customer _customerDomain = new(config, _cache);
        var searchresult = _customerDomain.GetByField(FieldName: "email", FieldValue: customer.emailaddress1);

        if (searchresult != null && searchresult.customer_id > 0)
        {
            return searchresult.customer_id;
        }


        //Add Unique Key in Sales Channel Logic here

        return default;
    }

    private KTI.Moo.Extensions.Magento.Model.Address GetAddress1FromCrm(KTI.Moo.Extensions.Magento.Service.Config config, Domain.Models.Customer.Customer customer)
    {

        return new()
        {
            //crmCustomer.firstname
            first_name = customer.firstname,
            //crmCustomer.lastname
            last_name = customer.lastname,
            //crmCustomer.firstname
            defaultBilling = true,
            defaultShipping = string.IsNullOrWhiteSpace(customer.address2_line1) && string.IsNullOrWhiteSpace(customer.address3_line1),
            //crmCustomer.firstname
            telephone = string.IsNullOrWhiteSpace(customer.address1_telephone1) ? customer.mobilephone : customer.address1_telephone1,
            address_line1 = customer.address1_line1,
            address_line2 = customer.address1_line2,
            address_line3 = customer.address1_line3,
            //crmCustomer.address1_line1, crmCustomer.address1_line2, crmCustomer.address1_line3 
            //street = HelperToArray("123 okay st"),
            //crmCustomer.address1_city
            address_city = customer.address1_city,
            //GetCountryID(crmCustomer.address_country)
            country_id = "PH",//GetCountryID(customer.address1_country),
            //crmCustomer.address1_postalcode
            address_postalcode = customer.address1_postalcode,
            //region not required anymore
            region = new()
            {
                //getregionid(crmcustomer.address1_stateorprovince)
                region_id = GetRegionID(config, customer.address1_stateorprovince)
            },
            custom_attributes = new()
            {
                new()
                {
                    attribute_code = "email",
                    value = customer.emailaddress1
                }
            }
        };

    }

    private KTI.Moo.Extensions.Magento.Model.Address GetAddress2FromCrm(KTI.Moo.Extensions.Magento.Service.Config config, Domain.Models.Customer.Customer customer)
    {

        return new()
        {
            //crmCustomer.firstname
            first_name = customer.firstname,
            //crmCustomer.lastname
            last_name = customer.lastname,
            //crmCustomer.firstname
            defaultShipping = true,
            defaultBilling = false,
            //crmCustomer.firstname
            telephone = string.IsNullOrWhiteSpace(customer.address2_telephone1) ? customer.mobilephone : customer.address2_telephone1,
            address_line1 = customer.address2_line1,
            address_line2 = customer.address2_line2,
            address_line3 = customer.address2_line3,
            //crmCustomer.address1_line1, crmCustomer.address1_line2, crmCustomer.address1_line3 
            //street = HelperToArray("123 okay st"),
            //crmCustomer.address1_city
            address_city = customer.address2_city,
            //GetCountryID(crmCustomer.address_country)
            country_id = "PH", // GetCountryID(customer.address2_country),
            //crmCustomer.address1_postalcode
            address_postalcode = customer.address2_postalcode,
            //region not required anymore
            region = new()
            {
                //GetRegionID(crmCustomer.address1_stateorprovince)
                region_id = GetRegionID(config, customer.address2_stateorprovince)
            },
            custom_attributes = new()
            {
                new()
                {
                    attribute_code = "email",
                    value = string.IsNullOrWhiteSpace(customer.emailaddress2) ? customer.emailaddress1 : customer.emailaddress2
                }

            }

        };

    }

    private KTI.Moo.Extensions.Magento.Model.Address GetAddress3FromCrm(KTI.Moo.Extensions.Magento.Service.Config config, Domain.Models.Customer.Customer customer)
    {

        return new()
        {
            //crmCustomer.firstname
            first_name = customer.firstname,
            //crmCustomer.lastname
            last_name = customer.lastname,
            //crmCustomer.firstname
            defaultShipping = false,
            defaultBilling = false,
            //crmCustomer.firstname
            telephone = string.IsNullOrWhiteSpace(customer.address3_telephone1) ? customer.mobilephone : customer.address3_telephone1,
            address_line1 = customer.address3_line1,
            address_line2 = customer.address3_line2,
            address_line3 = customer.address3_line3,
            //crmCustomer.address1_line1, crmCustomer.address1_line2, crmCustomer.address1_line3 
            //street = HelperToArray("123 okay st"),
            //crmCustomer.address1_city
            address_city = customer.address3_city,
            //GetCountryID(crmCustomer.address_country)
            country_id = "PH",  //GetCountryID(customer.address3_country),
            //crmCustomer.address1_postalcode
            address_postalcode = customer.address3_postalcode,
            //region not required anymore
            region = new()
            {
                //GetRegionID(crmCustomer.address1_stateorprovince)
                region_id = GetRegionID(config, customer.address3_stateorprovince)
            },
            custom_attributes = new()
            {
                new()
                {
                    attribute_code = "email",
                    value = string.IsNullOrWhiteSpace(customer.emailaddress3) ? customer.emailaddress1 : customer.emailaddress3
                }

            }

        };

    }

    public string GetRegionID(KTI.Moo.Extensions.Magento.Service.Config config, string region_name)
    {
        KTI.Moo.Extensions.Magento.Domain.Country _countryDomain = new(config);

        return _countryDomain.GetRegionID(region_name);
    }

    public string GetCountryID(KTI.Moo.Extensions.Magento.Service.Config config, string CountryName)
    {
        KTI.Moo.Extensions.Magento.Domain.Country _countryDomain = new(config);
        return _countryDomain.GetCountryID(CountryName);

    }


}
