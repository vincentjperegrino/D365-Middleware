using System.Text.Json;

namespace KTI.Moo.Extensions.Lazada.Service
{
    public interface ILazopService : Core.Service.IntegrationServiceBase
    {
        JsonSerializerOptions serializerOptions { get; set; }
        string AppKey { get; set; }
        string AppSecret { get; set; }
        string SellerId { get; set; }
        Lazada.Model.ClientTokens ClientTokens { get; set; }
    }
}
