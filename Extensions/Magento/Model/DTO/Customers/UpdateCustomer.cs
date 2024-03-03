using KTI.Moo.Extensions.Magento.Model.DTO.Customers.Base;

namespace KTI.Moo.Extensions.Magento.Model.DTO.Customers
{
    internal class Update : CustomerbaseDTO
    {
        public string passwordHash { get; set; }

    }
}
