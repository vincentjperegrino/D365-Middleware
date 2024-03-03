namespace KTI.Moo.ChannelApps.Core.Domain;
public interface IProduct<GenericProduct> where GenericProduct : CRM.Model.ProductBase
{
    bool DefautProcess(string decodedJsonString);

    GenericProduct GetClientModel(string decodedJsonString);

    string GetJsonForMessageQueue(GenericProduct clientModel);

    bool SendMessageToQueue(string Json);
}
