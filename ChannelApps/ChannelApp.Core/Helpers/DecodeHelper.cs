
namespace KTI.Moo.ChannelApps.Core.Helpers;

public static class Decode
{
    public static string Base64(this string value)
    {
        var valueBytes = System.Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(valueBytes);
    }
}
