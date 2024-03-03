using KTI.Moo.Extensions.Magento.Model.DTO.Customers.Base;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Customers.ForDispatchers
{
    internal class Add
    {

        public dynamic customer { get; set; }

        public string password { get; set; }

        public string redirectUrl { get; set; }

    }
}
