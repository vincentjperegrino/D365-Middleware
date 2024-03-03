namespace KTI.Moo.ChannelApps.Core.Domain;

public interface ICustomer<GenericCustomer>  where GenericCustomer : CRM.Model.CustomerBase
{

    bool DefautProcess(string decodedJsonString);

    GenericCustomer GetClientModelCustomer(string decodedJsonString);

    string GetJsonForMessageQueue(object clientModel);

    bool SendMessageToQueue(string Json, string QueueName);

}
