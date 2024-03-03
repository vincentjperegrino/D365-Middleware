
using System;
using Microsoft.Xrm.Sdk;
using CRM_Plugin.Domain;

namespace CRM_Plugin.Function
{
    public class SalesChannelHashingAndEncryption
    {
        private readonly ITracingService _tracingService;
        private readonly ISalesChannel _salesChannel;

        public SalesChannelHashingAndEncryption(ISalesChannel salesChannel, ITracingService tracingService)
        {
            _tracingService = tracingService;
            _salesChannel = salesChannel;
        }


        public bool ProcessAppPassword(Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                return EncryptPassword(salesChannel);

            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
            }
        }

        public bool ProcessAppKey(Models.ChannelManagement.SalesChannel salesChannel) 
        {
            try
            {
                return EncryptAppKey(salesChannel);

            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting key: " + ex.Message);
            }
        }

        public bool ProcessAppSecret(Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                return EncryptAppSecret(salesChannel);

            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting key: " + ex.Message);
            }
        }

        private bool HashPassword(Models.ChannelManagement.SalesChannel salesChannel)
        {
            return _salesChannel.HashPassword(salesChannel);
        }      

        private bool EncryptPassword(Models.ChannelManagement.SalesChannel salesChannel)
        {
            return _salesChannel.EncryptPassword(salesChannel);
        }
        private bool EncryptAppKey(Models.ChannelManagement.SalesChannel salesChannel)
        {
            return _salesChannel.EncryptAppKey(salesChannel);
        }

        private bool EncryptAppSecret(Models.ChannelManagement.SalesChannel salesChannel)
        {
            return _salesChannel.EncryptAppSecret(salesChannel);
        }

    }
}


