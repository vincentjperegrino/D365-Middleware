using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Domain.Dispatchers;

public interface IReplicate 
{
    bool Replicate(string id);

}
