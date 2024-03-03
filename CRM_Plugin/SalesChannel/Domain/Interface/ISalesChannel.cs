

namespace CRM_Plugin.Domain
{
    public interface ISalesChannel
    {
        bool HashPassword(Models.ChannelManagement.SalesChannel salesChannel);
        bool EncryptPassword(Models.ChannelManagement.SalesChannel salesChannel);
        bool EncryptAppKey(Models.ChannelManagement.SalesChannel salesChannel);
        bool EncryptAppSecret(Models.ChannelManagement.SalesChannel salesChannel);
    }
}
