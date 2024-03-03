using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCyware.App.Helper
{
    public class CalculationHelper
    {
        public class Item
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Discount { get; set; }
        }
        public static decimal CalculateTotalAmount(List<Item> items)
        {
            decimal total = 0.00m;
            foreach (var item in items)
            {
                decimal itemTotal = item.Price * item.Quantity;
                if (item.Discount > 0)
                {
                    itemTotal -= item.Discount;
                }
                total += itemTotal;
            }
            return total;
        }

    }
}
