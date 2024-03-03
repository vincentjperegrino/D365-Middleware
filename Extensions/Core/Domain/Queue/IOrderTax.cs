using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain.Queue;

public interface IOrderTax<T> where T : Model.OrderBase
{
    public T GetTax_Exclusive(string orderid);
    public T GetTax_Inclusive(string orderid);

}