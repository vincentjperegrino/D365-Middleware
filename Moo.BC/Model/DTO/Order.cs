using Moo.BC.Model.Extension;

namespace Moo.BC.Model.DTO
{
    public class Order
    {
        public List<OrderHeader> order_header { get; set; }
        public List<OrderLine> order_line { get; set; }
        public List<PaymentMethod> payment_method { get; set; }
    }
}
