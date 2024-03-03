
using Azure.Storage.Queues;
using KTI.Moo.Base.Helpers;
using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.SAP;

public class Customer : ICustomerToQueue
{

    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {

        var configFromQueueObject = JsonConvert.DeserializeObject<JObject>(messagequeue);

        var configFromQueueSalesChannelObject = configFromQueueObject[KTI.Moo.Base.Helpers.ChannelMangement.saleschannelConfigDTOname];

        var saleschannel = JsonConvert.SerializeObject(configFromQueueSalesChannelObject);

        var configFromQueueSalesChannel = JsonConvert.DeserializeObject<CRM.Model.ChannelManagement.SalesChannel>(saleschannel);

        KTI.Moo.Extensions.SAP.Service.Config Config = new()
        {
            defaultURL = configFromQueueSalesChannel.kti_defaulturl,
            redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False",
            password = configFromQueueSalesChannel.kti_password,
            username = configFromQueueSalesChannel.kti_username,
            companyDB = configFromQueueSalesChannel.kti_databasename,
            companyid = CompanyID,
        };

        var customer = JsonConvert.DeserializeObject<Domain.Models.Customer.Customer>(messagequeue);

        KTI.Moo.Extensions.SAP.Domain.Customer _SAPCustomerDomain = new(Config);

        KTI.Moo.Extensions.SAP.Model.DTO.Customers.Upsert SAPCustomer = new();


        //IF SAP
        SAPCustomer.kti_sapbpcode = GetBPcode(_SAPCustomerDomain, customer);

        if (string.IsNullOrWhiteSpace(customer.kti_sapbpcode))
        {
            SAPCustomer.contactid = customer.contactid;
        }


        if (string.IsNullOrWhiteSpace(SAPCustomer.kti_sapbpcode))
        {

            if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_magento)
            {
                SAPCustomer.Series = 101;
            }
            if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos)
            {
                SAPCustomer.Series = 101;
            }
            if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_lazada)
            {
                SAPCustomer.Series = 148;
            }

        }


        SAPCustomer.EmailAddress = customer.emailaddress1;
        SAPCustomer.CardName = $"{customer.firstname} {customer.lastname}";
        SAPCustomer.mobilephone = customer.mobilephone;

        if (!string.IsNullOrWhiteSpace(customer.ncci_clubmembershipid))
        {
            SAPCustomer.MemberCode = customer.ncci_clubmembershipid.TrimEnd();
        }

        if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_magento)
        {
            SAPCustomer.Channel = "Magento";
        }

        if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_octopos)
        {
            SAPCustomer.Channel = "Octopus";
        }

        if (customer.kti_socialchannelorigin == CRM.Helper.ChannelOrigin.OptionSet_lazada)
        {
            SAPCustomer.Channel = "Lazada";
        }


        SAPCustomer.Addresses = new();

        if (!string.IsNullOrWhiteSpace(customer.address1_line1))
        {
            SAPCustomer.Addresses.Add(GetAddress1FromCrm(customer));
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_line1))
        {
            SAPCustomer.Addresses.Add(GetAddress2FromCrm(customer));
        }

        if (!string.IsNullOrWhiteSpace(customer.address3_line1))
        {
            SAPCustomer.Addresses.Add(GetAddress3FromCrm(customer));
        }

        return SendMessageToQueue(SAPCustomer, QueueName, QueueConnectionString);
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

    private string GetBPcode(KTI.Moo.Extensions.SAP.Domain.Customer SAPCustomerDomain, Domain.Models.Customer.Customer customer)
    {
        if (customer.kti_socialchannelorigin == 959080009)
        {
            return customer.kti_sourceid;
        }

        if (!string.IsNullOrWhiteSpace(customer.kti_sapbpcode))
        {
            return customer.kti_sapbpcode;
        }

        //Add Unique Key in Sales Channel Logic here

        var ReturnCustomerData = SAPCustomerDomain.GetByField(FieldName: "EmailAddress", FieldValue: customer.emailaddress1);

        if (!string.IsNullOrWhiteSpace(ReturnCustomerData.kti_sapbpcode))
        {
            return ReturnCustomerData.kti_sapbpcode;
        }

        if (!string.IsNullOrWhiteSpace(ReturnCustomerData.mobilephone) && ReturnCustomerData.mobilephone.Length > 9)
        {

            ReturnCustomerData = SAPCustomerDomain.GetByField("Cellular", customer.mobilephone);

            if (!string.IsNullOrWhiteSpace(ReturnCustomerData.kti_sapbpcode))
            {
                return ReturnCustomerData.kti_sapbpcode;
            }
        }


        return customer.kti_sapbpcode;
    }


    private KTI.Moo.Extensions.SAP.Model.Address GetAddress1FromCrm(Domain.Models.Customer.Customer customer)
    {

        return new()
        {

            RowNum = 0,
            AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Billing,
            address_line1 = customer.address1_line1.TrimFirst100Characters(),
            address_postalcode = customer.address1_postalcode,
            address_city = customer.address1_city,
            address_country = customer.address1_country == "Philippines" ? "PH" : customer.address1_country

        };

    }

    private KTI.Moo.Extensions.SAP.Model.Address GetAddress2FromCrm(Domain.Models.Customer.Customer customer)
    {

        return new()
        {

            RowNum = 1,
            AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Shipping,
            address_line1 = customer.address1_line2.TrimFirst100Characters(),
            address_postalcode = customer.address2_postalcode,
            address_city = customer.address2_city,
            address_country = customer.address2_country == "Philippines" ? "PH" : customer.address2_country
        };

    }


    private KTI.Moo.Extensions.SAP.Model.Address GetAddress3FromCrm(Domain.Models.Customer.Customer customer)
    {

        return new()
        {
            RowNum = 2,
            AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Shipping,
            address_line1 = customer.address3_line2.TrimFirst100Characters(),
            address_postalcode = customer.address3_postalcode,
            address_city = customer.address3_city,
            address_country = customer.address3_country == "Philippines" ? "PH" : customer.address3_country
        };

    }


}
