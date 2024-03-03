using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Model.DTO.ChannelManagement
{
    public class CustomField : Core.Model.ChannelMangement.CustomFieldBase
    {
        public CustomField()
        {

        }
        public CustomField(Entity entity)
        {

            string ErrorEntity = "";

            try
            {
                ErrorEntity = "kti_channelfieldname";
                if (entity.Contains("kti_channelfieldname"))
                {
                    this.kti_channelfieldname = (string)entity["kti_channelfieldname"];
                }

                ErrorEntity = "kti_crmfieldname";
                if (entity.Contains("kti_crmfieldname"))
                {
                    this.kti_crmfieldname = (string)entity["kti_crmfieldname"];
                }

                ErrorEntity = "kti_crmtable";
                if (entity.Contains("kti_crmtable"))
                {
                    this.kti_crmtable = ((OptionSetValue)entity["kti_crmtable"]).Value;
                }

                ErrorEntity = "kti_customfieldmappingId";
                if (entity.Contains("kti_customfieldmappingId"))
                {
                    this.kti_customfieldmappingId = ((EntityReference)entity["kti_customfieldmappingId"]).Id.ToString();
                }

                ErrorEntity = "kti_customfieldname";
                if (entity.Contains("kti_customfieldname"))
                {
                    this.kti_customfieldname = (string)entity["kti_customfieldname"];
                }

                ErrorEntity = "kti_customvaluename";
                if (entity.Contains("kti_customvaluename"))
                {
                    this.kti_customvaluename = (string)entity["kti_customvaluename"];
                }

                ErrorEntity = "kti_hasparentfield";
                if (entity.Contains("kti_hasparentfield"))
                {
                    this.kti_hasparentfield = (bool)entity["kti_hasparentfield"];
                }

                ErrorEntity = "kti_parentfieldname";
                if (entity.Contains("kti_parentfieldname"))
                {
                    this.kti_parentfieldname = (string)entity["kti_parentfieldname"];
                }

                ErrorEntity = "kti_iscustomfieldwithnameandvalue";
                if (entity.Contains("kti_iscustomfieldwithnameandvalue"))
                {
                    this.kti_iscustomfieldwithnameandvalue = (bool)entity["kti_iscustomfieldwithnameandvalue"];
                }

                ErrorEntity = "kti_isstatic";
                if (entity.Contains("kti_isstatic"))
                {
                    this.kti_isstatic = (bool)entity["kti_isstatic"];
                }

                ErrorEntity = "kti_isuniquekey";
                if (entity.Contains("kti_isuniquekey"))
                {
                    this.kti_isuniquekey = (bool)entity["kti_isuniquekey"];
                }

                ErrorEntity = "kti_name";
                if (entity.Contains("kti_name"))
                {
                    this.kti_name = (string)entity["kti_name"];
                }

                ErrorEntity = "kti_saleschannel";
                if (entity.Contains("kti_saleschannel"))
                {
                    this.kti_saleschannel = ((EntityReference)entity["kti_saleschannel"]).Id.ToString();
                }

                ErrorEntity = "kti_saveoninsert";
                if (entity.Contains("kti_saveoninsert"))
                {
                    this.kti_saveoninsert = (bool)entity["kti_saveoninsert"];
                }

                ErrorEntity = "kti_staticvalue";
                if (entity.Contains("kti_staticvalue"))
                {
                    this.kti_staticvalue = (string)entity["kti_staticvalue"];
                }

                ErrorEntity = "kti_datatype";
                if (entity.Contains("kti_datatype"))
                {
                    this.kti_datatype = ((OptionSetValue)entity["kti_datatype"]).Value;
                }
            }
            catch
            {
                throw new Exception("CustomField: Invalid " + ErrorEntity);
            }


        }
    }
}
