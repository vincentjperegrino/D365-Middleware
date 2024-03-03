using KTI.Moo.Extensions.Core.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain;
public interface IPrices<T> where T : PriceHeaderBase
{ 
    // basic crud methods
    T Get(int customerID);
    T Get(string customerID);
    T Add(T customerDetails);
    bool Update(T customerDetails);
    bool Delete(int customerID);
    T Upsert(T priceheader);//string message, string rootfolder);
}

