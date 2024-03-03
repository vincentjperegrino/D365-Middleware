namespace KTI.Moo.ChannelApps.Core.Domain;
public interface IOrder<GenericOrder, GenericCustomer , GenericInvoice> where GenericOrder : CRM.Model.OrderBase where GenericCustomer : CRM.Model.CustomerBase where GenericInvoice : CRM.Model.InvoiceBase
{
    bool DefautProcess(string decodedJsonString);

    bool WithCustomerProcess(string decodedJsonString);

    bool WithCustomer_Invoice_Process(string decodedJsonString);

    GenericOrder GetClientModelOrder(string decodedJsonString);

    GenericCustomer GetClientModelCustomer(string decodedJsonString);

    GenericInvoice GetClientModelInvoice(string decodedJsonString);

    string GetJsonForMessageQueue(object clientModel);

    bool SendMessageToQueue(string Json, string QueueName);

}
