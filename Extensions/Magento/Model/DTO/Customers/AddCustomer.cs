using KTI.Moo.Extensions.Magento.Model.DTO.Customers.Base;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Customers
{
    internal class Add : CustomerbaseDTO
    {

        public string password { get; set; }

        public string redirectUrl { get; set; }

    }
}
