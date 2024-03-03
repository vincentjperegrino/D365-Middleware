
using System;
using Microsoft.Xrm.Sdk;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using CRM_Plugin.Domain;
using Core_EntityHelper = CRM_Plugin.Core.Helper.EntityHelper;
using _helper = CRM_Plugin.Core.Helper.HashingAndEncryption;
using Microsoft.Xrm.Sdk.Query;

namespace CRM_Plugin.SalesChannel.Domain
{
    public class SalesChannel : ISalesChannel
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public SalesChannel(IOrganizationService service, ITracingService tracingService)
        {
            _service = service;
            _tracingService = tracingService;
            
        }
        public SalesChannel()
        {

        }

        public EntityCollection GetSalesChannelByProductID(Guid productID)
        {
            try
            {
                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "kti_saleschannelproduct";
                qeEntity.ColumnSet = new ColumnSet("kti_saleschannel", "kti_product");

                var entityFilter = new FilterExpression(LogicalOperator.And);

                entityFilter.AddCondition("kti_product", ConditionOperator.Equal, productID.ToString());

                qeEntity.Criteria.AddFilter(entityFilter);

                LinkEntity lnkSalesChannel = new LinkEntity("kti_saleschannelproduct", "kti_saleschannel", "kti_saleschannel", "kti_saleschannelid", JoinOperator.Inner);

                lnkSalesChannel.Columns = new ColumnSet(true);

                lnkSalesChannel.EntityAlias = "sc";

                qeEntity.LinkEntities.Add(lnkSalesChannel);

                return _service.RetrieveMultiple(qeEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("GetSalesChannelByProductID: " + ex.Message);
            }
        }

        public EntityCollection GetSalesChannelByChannel(OptionSetValue optionSetValue)
        {
            try
            {
                var qeEntity = new QueryExpression();
                qeEntity.EntityName = "kti_saleschannel";
                qeEntity.ColumnSet = new ColumnSet(true);

                var entityFilter = new FilterExpression(LogicalOperator.And);

                entityFilter.AddCondition("kti_channelorigin", ConditionOperator.Equal, optionSetValue.Value); //SRP

                qeEntity.Criteria.AddFilter(entityFilter);

                return _service.RetrieveMultiple(qeEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
            }
        }

        public Entity GetSalesChannelByID(Guid salesChannelID)
        {
            try
            {
                return _service.Retrieve("kti_saleschannel", salesChannelID, new ColumnSet(true));
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
            }
        }

        public bool HashPassword(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                var salt = GenerateRandomNumber(_helper.HashingAndEncryption._saltLength);
                var hashedValue = GenerateHash(salesChannel, salt);

                return UpdatePassword(salesChannel, hashedValue, salt);

                return false;
            } 
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
            }
        }

        public bool EncryptPassword(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                var encryptedValue = EncryptKey(salesChannel.password);

                if (CompareEncryptedValues(salesChannel.password, encryptedValue))
                {
                    return UpdatePassword(salesChannel, encryptedValue);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting password: " + ex.Message);
            }
        }

        public bool EncryptAppKey(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                var encryptedValue = EncryptKey(salesChannel.appKey);

                if (CompareEncryptedValues(salesChannel.appKey, encryptedValue))
                {
                    return UpdateAppKey(salesChannel, encryptedValue);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting app key: " + ex.Message);
            }

        }

        public bool EncryptAppSecret(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel)
        {
            try
            {
                var encryptedValue = EncryptKey(salesChannel.appSecret);

                if (CompareEncryptedValues(salesChannel.appSecret, encryptedValue))
                {
                    return UpdateAppSecret(salesChannel, encryptedValue);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting app secret: " + ex.Message);
            }

        }

        private static byte[] GenerateRandomNumber(int length)
        {
            var numberGenerator = RandomNumberGenerator.Create();
            var size = length;
            var salt = new byte[size];
            numberGenerator.GetBytes(salt);
            return salt;
        }

        private string GenerateHash(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel, byte[] salt)
        {
            var pbkdf2Key = new Rfc2898DeriveBytes(salesChannel.password + _helper.HashingAndEncryption.salesChannel_pepper, salt, _helper.HashingAndEncryption._iterationCount);

            return Convert.ToBase64String(pbkdf2Key.GetBytes(_helper.HashingAndEncryption._hashSize));
        }

        private bool CompareHashedValues(CRM_Plugin.Models.ChannelManagement.SalesChannel salesChannel, string hashedValue, byte[] salt)
        {

            var _hashedPassword = GenerateHash(salesChannel, salt);
            var isEqual = _hashedPassword.SequenceEqual(hashedValue);

            if (!isEqual)
            {
                throw new Exception("Error saving password");
            }

            return true;
        }

        private string EncryptKey(string appKey)
        {

            try
            {
                var buffer = Encoding.UTF8.GetBytes(appKey);
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(_helper.HashingAndEncryption._key);
                aes.IV = new byte[_helper.HashingAndEncryption._IV];

                var resultStream = new MemoryStream();
                var cryptoStream = new CryptoStream(resultStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);

                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(resultStream.ToArray());


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string DecryptKey(string encryptedValue)
        {

            try
            {
                var buffer = Convert.FromBase64String(encryptedValue);
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(_helper.HashingAndEncryption._key);
                aes.IV = new byte[_helper.HashingAndEncryption._IV];

                var resultStream = new MemoryStream();
                var cryptoStream = new CryptoStream(resultStream, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Write);

                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(resultStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool CompareEncryptedValues(string inputKey, string encryptedValue)
        {
            var _decryptedValue = DecryptKey(encryptedValue); 
            var isEqual = _decryptedValue.Equals(inputKey);

            if (!isEqual)
            {
                throw new Exception("Keys do not match");
            }

            return true;
        }
        private bool UpdatePassword(CRM_Plugin.Models.ChannelManagement.SalesChannel _salesChannel, string encryptedValue)
        {
            try
            {
                var salesChannel = new Entity(Core_EntityHelper.SalesChannel.entity_name)

                {
                    Id = _salesChannel.salesChannel.Id
                };

                salesChannel[Core_EntityHelper.SalesChannel.password] = encryptedValue;
                salesChannel[Core_EntityHelper.SalesChannel.passwordFlag] = encryptedValue;

                _service.Update(salesChannel);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool UpdatePassword(CRM_Plugin.Models.ChannelManagement.SalesChannel _salesChannel, string hashedValue, byte[] salt)
        {
            try
            {
                var salesChannel = new Entity(Core_EntityHelper.SalesChannel.entity_name)

                {
                    Id = _salesChannel.salesChannel.Id
                };

                salesChannel[Core_EntityHelper.SalesChannel.password] = hashedValue;
                salesChannel[Core_EntityHelper.SalesChannel.passwordFlag] = hashedValue;
                salesChannel[Core_EntityHelper.SalesChannel.salt] = Convert.ToBase64String(salt);

                _service.Update(salesChannel);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool UpdateAppKey(CRM_Plugin.Models.ChannelManagement.SalesChannel _salesChannel, string encryptedValue)
        {
            try
            {
                var salesChannel = new Entity(Core_EntityHelper.SalesChannel.entity_name)

                {
                    Id = _salesChannel.salesChannel.Id
                };

                salesChannel[Core_EntityHelper.SalesChannel.appKey] = encryptedValue;
                salesChannel[Core_EntityHelper.SalesChannel.appKeyFlag] = encryptedValue;

                _service.Update(salesChannel);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool UpdateAppSecret(CRM_Plugin.Models.ChannelManagement.SalesChannel _salesChannel, string encryptedValue)
        {
            try
            {
                var salesChannel = new Entity(Core_EntityHelper.SalesChannel.entity_name)

                {
                    Id = _salesChannel.salesChannel.Id
                };

                salesChannel[Core_EntityHelper.SalesChannel.appSecret] = encryptedValue;
                salesChannel[Core_EntityHelper.SalesChannel.appSecretFlag] = encryptedValue;

                _service.Update(salesChannel);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}


