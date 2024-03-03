using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;
using System;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface ICustomer<T> where T : CustomerBase
    {
        // basic crud methods
        T Get(int customerID);
        T Get(string customerID);
        T Add(T customerDetails);
        bool Update(T customerDetails);
        bool Delete(int customerID);
        T Upsert(T customerDetails);
      
    }
}