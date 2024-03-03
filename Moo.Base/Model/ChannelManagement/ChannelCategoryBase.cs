using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Model.ChannelManagement;

public class ChannelCategoryBase : CustomEntityBase
{
    public string kti_channelcategorymappingid { get; set; }
    public string kti_name { get; set; }
    public string kti_description { get; set; }
    public string kti_saleschannel { get; set; }
    public string kti_channelcategory { get; set; }
    public string channelcategoryProductFamilyCode { get; set; }
    public string kti_sourcecategory { get; set; }
    public string sourcecategoryProductFamilyCode { get; set; }
}
