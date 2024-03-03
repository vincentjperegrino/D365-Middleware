using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Domain
{
    public interface IConfigurationGroup<T> where T : ConfigurationGroupBase
    {  
        
        // basic crud methods
        T Get(int configGroupID);
        T Get(string configGroupID);
        T Add(T configGroupDetails);
        bool Update(T configGroupDetails);
        bool Delete(int configGroupID);
        T Upsert(T configGroupDetails);

    }
}
