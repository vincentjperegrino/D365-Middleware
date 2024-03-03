using KTI.Moo.ChannelApps.Core.Domain.Receivers;
using KTI.Moo.ChannelApps.Model.NCCI.Receivers;
using KTI.Moo.Extensions.Core.Helper;
using KTI.Moo.Extensions.Lazada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.OctoPOS.Implementation.NCCI;

public class Order : Core.Domain.IOrder<ChannelApps.Model.NCCI.Receivers.Plugin.Order, ChannelApps.Model.NCCI.Receivers.Plugin.Customer, ChannelApps.Model.NCCI.Receivers.Invoice>, IOrderForReceiver
{
    private readonly string _connectionString;
    private readonly string _companyId;

    public Order(string connectionString, string companyId)
    {
        _connectionString = connectionString;
        _companyId = companyId;
    }

    public bool DefautProcess(string decodedJsonString)
    {
        var Customer = GetClientModelCustomer(decodedJsonString);
        var Order = GetClientModelOrder(decodedJsonString);
        var OrderItem = GetClientModelOrderItem(decodedJsonString);

        var DTO = new CRM.Model.DTO.Orders.OrderWithCustomer<ChannelApps.Model.NCCI.Receivers.Plugin.Customer, ChannelApps.Model.NCCI.Receivers.Plugin.Order, ChannelApps.Model.NCCI.Receivers.Plugin.OrderItem>()
        {
            companyid = int.Parse(_companyId),
            Customer = Customer,
            Order = Order,
            OrderLine = OrderItem
        };

        var Json = GetJsonForMessageQueue(DTO);

        return SendMessageToQueue(Json, $"{_companyId}{Core.Helpers.QueueName.Order}-migration");
    }

    public bool WithCustomerProcess(string decodedJsonString)
    {

        var Customer = GetClientModelCustomer(decodedJsonString);
        var Order = GetClientModelOrder(decodedJsonString);
        var OrderItem = GetClientModelOrderItem(decodedJsonString);

        var DTO = new CRM.Model.DTO.Orders.OrderWithCustomer<ChannelApps.Model.NCCI.Receivers.Plugin.Customer , ChannelApps.Model.NCCI.Receivers.Plugin.Order , ChannelApps.Model.NCCI.Receivers.Plugin.OrderItem>()
        {
            companyid = int.Parse(_companyId),
            Customer = Customer,
            Order = Order,
            OrderLine = OrderItem
        };

        var Json = GetJsonForMessageQueue(DTO);

        return SendMessageToQueue(Json, $"{_companyId}{Core.Helpers.QueueName.Order}");
    }


    public bool WithCustomer_Invoice_Process(string decodedJsonString)
    {
        throw new NotImplementedException();
    }


    public Model.NCCI.Receivers.Plugin.Customer GetClientModelCustomer(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.OctoPOS.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);
        var CustomerModel = channelAppsInvoice.customer;

        if (CustomerModel is null)
        {
            throw new Exception("Record invalid.");
        }

        var company = CustomerModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        var Customer = new Model.NCCI.Receivers.Plugin.Customer(CustomerModel);


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

        var CustomerList = customer.GetContactListByChannel_SourceID_Email_MobileNumber_FirstNameLastName(CustomerModelForSearching).GetAwaiter().GetResult();

        var Contactid = "";

        if (CustomerList is not null && CustomerList.Count > 0)
        {
  
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

    public Model.NCCI.Receivers.Invoice GetClientModelInvoice(string decodedJsonString)
    {
        throw new NotImplementedException();
    }

    public Model.NCCI.Receivers.Plugin.Order GetClientModelOrder(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.OctoPOS.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);
        var OrderModel = channelAppsInvoice.invoice;

        if (OrderModel is null)
        {
            throw new Exception("Record invalid.");
        }

        var company = OrderModel.companyid;

        if (company == 0)
        {
            throw new Exception("Attribute companyid missing.");
        }

        return new Model.NCCI.Receivers.Plugin.Order(OrderModel);

    }

    public List<Model.NCCI.Receivers.Plugin.OrderItem> GetClientModelOrderItem(string decodedJsonString)
    {
        var JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new Core.Helpers.JSONSerializer.DontIgnoreResolver()
        };

        var channelAppsInvoice = JsonConvert.DeserializeObject<Extensions.OctoPOS.Model.DTO.ChannelApps>(decodedJsonString, JsonSettings);

        var linenumber = 1;
        var OrderItemModel = channelAppsInvoice.invoice.InvoiceItems.Select(orderitem =>
        {
             
            var OrderItemModel = new Model.NCCI.Receivers.Plugin.OrderItem(orderitem , channelAppsInvoice.invoice.InvoiceType);
            OrderItemModel.kti_sourceid = channelAppsInvoice.invoice.kti_sourceid;
            OrderItemModel.kti_lineitemnumber = linenumber.ToString();
            linenumber++;
            return OrderItemModel;
        }).ToList();

        return OrderItemModel;

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
