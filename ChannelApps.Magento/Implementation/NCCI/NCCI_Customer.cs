using KTI.Moo.ChannelApps.Core.Domain;
using KTI.Moo.ChannelApps.Model.NCCI.Receivers.Plugin;
using KTI.Moo.Extensions.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Magento.Implementation.NCCI
{
    public class Customer : ICustomer<ChannelApps.Model.NCCI.Receivers.Customer>
    {
        private readonly string _companyId;
        private readonly string _connectionString;

        public Customer(string connectionString, string companyId)
        {
            _connectionString = connectionString;
            _companyId = companyId;
        }

        public bool DefautProcess(string decodedJsonString)
        {
            return CustomerProcess(decodedJsonString);
        }

        public Model.NCCI.Receivers.Customer GetClientModelCustomer(string decodedJsonString)
        {
            var JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
            };

            var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.Magento.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);

            var CustomerModel = channelAppsInvoice.customer;

            var company = CustomerModel.companyid;

            if (company == 0)
            {
                throw new Exception("Attribute companyid missing.");
            }

            var Customer = new Model.NCCI.Receivers.Customer(CustomerModel);


            CRM.Domain.Customer customer = new(CustomerModel.companyid);

            var CustomerModelForSearching = new KTI.Moo.CRM.Model.CustomerBase();

            CustomerModelForSearching.firstname = Customer.firstname;
            CustomerModelForSearching.lastname = Customer.lastname;

            CustomerModelForSearching.emailaddress1 = Customer.emailaddress1;
            CustomerModelForSearching.mobilephone = Customer.mobilephone;

            CustomerModelForSearching.address1_line1 = Customer.address1_line1;
            CustomerModelForSearching.address1_line2 = Customer.address1_line2;
            CustomerModelForSearching.address1_line3 = Customer.address1_line3;
            CustomerModelForSearching.address1_city = Customer.address1_city;
            CustomerModelForSearching.address1_postalcode = Customer.address1_postalcode;
            CustomerModelForSearching.address1_telephone1 = Customer.address1_telephone1;
            CustomerModelForSearching.address1_stateorprovince = Customer.address1_stateorprovince;
            CustomerModelForSearching.telephone1 = Customer.telephone1;
            CustomerModelForSearching.address1_country = Customer.address1_country;

            CustomerModelForSearching.address2_line1 = Customer.address2_line1;
            CustomerModelForSearching.address2_line2 = Customer.address2_line2;
            CustomerModelForSearching.address2_line3 = Customer.address2_line3;
            CustomerModelForSearching.address2_city = Customer.address2_city;
            CustomerModelForSearching.address2_postalcode = Customer.address2_postalcode;
            CustomerModelForSearching.address2_stateorprovince = Customer.address2_stateorprovince;
            CustomerModelForSearching.address2_country = Customer.address2_country;
            CustomerModelForSearching.address2_telephone1 = Customer.address2_telephone1;
            CustomerModelForSearching.telephone2 = Customer.telephone2;

            CustomerModelForSearching.kti_sourceid = Customer.kti_sourceid;
            CustomerModelForSearching.kti_socialchannelorigin = Customer.kti_socialchannelorigin;
            CustomerModelForSearching.kti_magentoid = Customer.kti_magentoid;

            // var Contactid = customer.GetContactBy_MagentoID_Mobile_Email(_customer.kti_sourceid, this.mobilephone, _customer.email).GetAwaiter().GetResult();
            var CustomerList = customer.GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName_WithMagentoID(CustomerModelForSearching).GetAwaiter().GetResult();


            var Contactid = "";

            if (CustomerList is not null && CustomerList.Count > 0)
            {
                if (CustomerList.Any(customer => customer.kti_magentoid == Customer.kti_magentoid))
                {
                    var SelectedCustomer = CustomerList.Where(customer => customer.kti_magentoid == Customer.kti_magentoid).FirstOrDefault();

                    Contactid = SelectedCustomer.contactid;

                    Customer.is_modified = SelectedCustomer.CompareTo(CustomerModelForSearching);

                }

                if (string.IsNullOrWhiteSpace(Contactid) && CustomerList.Any(customer => customer.kti_sourceid == Customer.kti_sourceid && customer.kti_socialchannelorigin == Customer.kti_socialchannelorigin))
                {
                    var SelectedCustomer = CustomerList.Where(customer => customer.kti_sourceid == Customer.kti_sourceid && customer.kti_socialchannelorigin == Customer.kti_socialchannelorigin).FirstOrDefault();

                    Contactid = SelectedCustomer.contactid;

                    Customer.is_modified = SelectedCustomer.CompareTo(CustomerModelForSearching);
                }

                if (string.IsNullOrWhiteSpace(Contactid))
                {
                    var SelectedCustomer = CustomerList.FirstOrDefault();

                    Contactid = SelectedCustomer.contactid;
                    Customer.is_modified = SelectedCustomer.CompareTo(CustomerModelForSearching);
                }
            }

            if (!string.IsNullOrWhiteSpace(Contactid))
            {
                Customer.contactid = Contactid;
            }
            else
            {
                Customer.contactid = default;
            }

            return Customer;
        }


        private bool CustomerProcess(string decodedJsonString)
        {
            var Customer = GetClientModelCustomer(decodedJsonString);

            if (!string.IsNullOrWhiteSpace(Customer.contactid) && Customer.is_modified == 0)
            {
                return false;
            }

            Customer.is_modified = default;
            var JSON = GetJsonForMessageQueue(Customer);
            var queueNameCustomer = $"{_companyId}{Core.Helpers.QueueName.Customer}";



            return SendMessageToQueue(JSON, queueNameCustomer);
        }

        public string GetJsonForMessageQueue(object clientModel)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };

            var Json = JsonConvert.SerializeObject(clientModel, Formatting.None, settings);

            return Json;
        }

        public bool SendMessageToQueue(string Json, string QueueName)
        {
            QueueClient queueClient = new QueueClient(_connectionString, QueueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.CreateIfNotExistsAsync().Wait();
            queueClient.SendMessage(Json.ToBrotliAsync().GetAwaiter().GetResult().Result.Value);

            return true;
        }
    }
}
