using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain.Queue
{
    public interface IProduct<T> : Domain.IProduct<T> where T : Core.Model.ProductBase
    {



    }
}
