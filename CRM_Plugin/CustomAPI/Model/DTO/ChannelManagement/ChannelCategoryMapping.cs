using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement
{
    public class ChannelCategoryMapping : Core.Model.ChannelMangement.ChannelCategoryMappingBase
    {
        public ChannelCategoryMapping()
        {

        }

        public ChannelCategoryMapping(Entity entity)
        {

            string ErrorEntity = "";

            try
            {
                ErrorEntity = "kti_channelcategorymappingid";
                if (entity.Contains("kti_channelcategorymappingid"))
                {
                    this.kti_channelcategorymappingid = ((Guid)entity["kti_channelcategorymappingid"]).ToString();
                }

                ErrorEntity = "kti_name";
                if (entity.Contains("kti_name"))
                {
                    this.kti_name = (string)entity["kti_name"];
                }

                ErrorEntity = "kti_description";
                if (entity.Contains("kti_description"))
                {
                    this.kti_description = (string)entity["kti_description"];
                }

                ErrorEntity = "kti_saleschannel";
                if (entity.Contains("kti_saleschannel"))
                {
                    this.kti_saleschannel = ((EntityReference)entity["kti_saleschannel"]).Id.ToString();
                }

                ErrorEntity = "kti_channelcategory";
                if (entity.Contains("kti_channelcategory"))
                {
                    this.kti_channelcategory = ((EntityReference)entity["kti_channelcategory"]).Id.ToString();
                }

                ErrorEntity = "kti_sourcecategory";
                if (entity.Contains("kti_sourcecategory"))
                {
                    this.kti_sourcecategory = ((EntityReference)entity["kti_sourcecategory"]).Id.ToString();
                }

            }
            catch
            {
                throw new Exception("ChannelCategoryMapping: Invalid " + ErrorEntity);
            }


        }
    }
}
