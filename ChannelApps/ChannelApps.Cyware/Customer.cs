using Azure.Storage.Queues;

namespace KTI.Moo.ChannelApps.Cyware;

public class Customer
{
    private readonly QueueClient _queueClient;

    public Customer(string connectionString)
    {
        _queueClient = new QueueClient(connectionString, "fo-customer-queue");
        _queueClient.CreateIfNotExists();
    }

    public bool customerProcess(string customerDetails)
    {
        return addCustomerToQueue(customerDetails);
    }

    public bool addCustomerToQueue(string customerDetails)
    {
        try
        {
            _queueClient.SendMessage(customerDetails);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
