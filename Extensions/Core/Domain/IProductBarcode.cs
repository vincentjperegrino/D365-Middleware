using KTI.Moo.Extensions.Core.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IProductBarcode<T> where T : ProductBarcodeBase
    {
        // basic crud methods
        T Get(int productID);
        T Get(string productID);
        T Add(T productBarcode);
        bool Update(T productBarcode);
        bool Delete(int productID);
        T Upsert(T productBarcode);
    }
}
