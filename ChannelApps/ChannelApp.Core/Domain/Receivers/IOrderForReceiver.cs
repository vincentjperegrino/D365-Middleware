using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Core.Domain.Receivers;

public interface IOrderForReceiver
{
    bool DefautProcess(string decodedJsonString);

    bool WithCustomerProcess(string decodedJsonString);

    bool WithCustomer_Invoice_Process(string decodedJsonString);


}
