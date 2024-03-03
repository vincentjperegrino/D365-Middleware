using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.Model.ChannelManagement;

public class SalesChannel : Base.Model.ChannelManagement.SalesChannelBase
{
    public List<CustomField> CustomFieldList { get; set; }
}
