using Azure.Storage.Queues;
using KTI.Moo.ChannelApps.Core.Domain.Dispatchers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.NCCI.Dispatchers.OctoPOS;

public class Customer : ICustomerToQueue
{


    public bool DispatchMessage(string messagequeue, string QueueName, string QueueConnectionString, string CompanyID)
    {

        var customer = JsonConvert.DeserializeObject<Domain.Models.Customer.Customer>(messagequeue);

        if (customer.kti_socialchannelorigin == KTI.Moo.CRM.Helper.ChannelOrigin.OptionSet_lazada)
        {
            return false; // Not for dispatch
        }

        KTI.Moo.Extensions.OctoPOS.Model.Customer CustomerModel = new();
        CustomerModel.contactid = customer.contactid;
        CustomerModel.CustomerCode = customer.kti_sourceid;
        CustomerModel.MembershipCode = customer.ncci_clubmembershipid;

        if (!string.IsNullOrWhiteSpace(customer.birthdate))
        {
            if (DateTime.TryParse(customer.birthdate, out var birthdates))
            {
                CustomerModel.Dob = birthdates;
            }
        }

        CustomerModel.firstname = customer.firstname;
        CustomerModel.lastname = customer.lastname;

        if (string.IsNullOrWhiteSpace(CustomerModel.firstname))
        {
            CustomerModel.firstname = CustomerModel.lastname;
        }

        if (customer.gendercode > 0)
        {
            CustomerModel.gendercode = customer.gendercode;
        }

        CustomerModel.Location = string.IsNullOrWhiteSpace(customer.ncci_customerjoinedbranch) ? "WELF01" : customer.ncci_customerjoinedbranch;
        CustomerModel.Email = customer.emailaddress1;
        CustomerModel.Address1 = customer.address1_line1;
        CustomerModel.Address2 = customer.address1_line2;
        CustomerModel.Address3 = customer.address1_city;
        CustomerModel.Country = "Philippines";
        CustomerModel.PostalCode = customer.address1_postalcode;
        CustomerModel.HandPhone = customer.mobilephone;
        CustomerModel.HomePhone = customer.telephone2;
        CustomerModel.salutation = customer.salutation;

        CustomerModel.ShippingAddress = customer.address2_line1;
        CustomerModel.ShippingAddress2 = customer.address2_line2;
        CustomerModel.ShippingCity = customer.address2_city;
        CustomerModel.ShippingPostalCode = customer.address2_postalcode;
        CustomerModel.ShippingCountry = "Philippines";
        CustomerModel.ShippingContact = customer.telephone2;
        CustomerModel.ModifiedDate = KTI.Moo.Extensions.OctoPOS.Helper.DateTimeHelper.PHTnow();

        if (string.IsNullOrWhiteSpace(customer.address2_line1))
        {
            CustomerModel.ShippingAddress = customer.address1_line1;
            CustomerModel.ShippingAddress2 = customer.address1_line2;
            CustomerModel.ShippingCity = customer.address1_city;
            CustomerModel.ShippingPostalCode = customer.address1_postalcode;
            CustomerModel.ShippingCountry = "Philippines";
            CustomerModel.ShippingContact = customer.mobilephone;

        }

        return SendMessageToQueue(CustomerModel, QueueName, QueueConnectionString);
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

}

