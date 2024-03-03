namespace KTI.Moo.ChannelApps.Core.Domain
{
    public interface IInvoice<GenericInvoice, GenericOrder , GenericCustomer> where GenericInvoice : CRM.Model.InvoiceBase where GenericOrder : CRM.Model.OrderBase where GenericCustomer : CRM.Model.CustomerBase
    {
        bool DefautProcess(string decodedJsonString);

        bool WithCustomerProcess(string decodedJsonString);

        bool With_Customer_Order_Process(string decodedJsonString);

        GenericInvoice GetClientModelInvoice(string decodedJsonString);

        GenericOrder GetClientModelOrder(string decodedJsonString);

        GenericCustomer GetClientModelCustomer(string decodedJsonString);

        string GetJsonForMessageQueue(object clientModel);

        bool SendMessageToQueue(string Json, string QueueName);

    }
}
