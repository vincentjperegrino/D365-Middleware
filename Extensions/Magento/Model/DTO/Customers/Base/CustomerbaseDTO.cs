using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Customers.Base
{
    public class CustomerbaseDTO
    {

        [JsonProperty("customer")]
        public Model.Customer Customer { get; set; }

    }
}
