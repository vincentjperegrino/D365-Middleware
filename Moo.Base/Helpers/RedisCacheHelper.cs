namespace KTI.Moo.Base.Helpers;

public class RedisCache
{
    public static string ChannelManagement = "channelmanagement";
    public static string GetChannelManagementStoreCode(string storecode) => $"{ChannelManagement}_{storecode}"; 
}
