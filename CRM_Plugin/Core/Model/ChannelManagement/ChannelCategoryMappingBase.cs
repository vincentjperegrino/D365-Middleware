using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.Core.Model.ChannelMangement
{
    public class ChannelCategoryMappingBase
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
}
