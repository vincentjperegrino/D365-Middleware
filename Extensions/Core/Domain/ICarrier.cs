using KTI.Moo.Extensions.Core.Model;
using System.Collections.Generic;
using System;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface ICarrier<T> where T : CarrierBase
    {
        // basic crud methods
        T Get(int carrierID);
        T Get(string carrierID);
        T Add(T carrierDetails);
        bool Update(T carrierDetails);
        bool Delete(int carrierID);
        T Upsert(T carrierDetails);

    }
}
