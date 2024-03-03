
using System;
using Microsoft.Xrm.Sdk;
using System.Security.Cryptography;
using System.Linq;
using CRM_Plugin.Domain;
using Core_EntityHelper = CRM_Plugin.Core.Helper.EntityHelper;
using _helper = CRM_Plugin.Core.Helper.HashingAndEncryption;

namespace CRM_Plugin.Employee.Domain
{
    public class Employee : IEmployee
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public Employee(IOrganizationService service, ITracingService tracingService)
        {
            _tracingService = tracingService;
            _service = service;
        }

        public bool HashPassword(Models.Employee employee)
        {
            try
            {
                var salt = GenerateRandomNumber(_helper.HashingAndEncryption._saltLength);
                var hashedValue = GenerateHash(employee, salt);

                return UpdatePassword(employee, hashedValue, salt);

            } 
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
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

        private string GenerateHash(Models.Employee employee, byte[] salt)
        {
            var pbkdf2Key = new Rfc2898DeriveBytes(employee.password + _helper.HashingAndEncryption.employee_pepper, salt, _helper.HashingAndEncryption._iterationCount);

            return Convert.ToBase64String(pbkdf2Key.GetBytes(_helper.HashingAndEncryption._hashSize));
        }


        private bool UpdatePassword(Models.Employee employee, string hashedValue, byte[] salt)
        {   
            try
            {
                var employeeRecord = new Entity(Core_EntityHelper.Employee.entity_name)

                {
                    Id = employee.employeeRecord.Id
                };

                employeeRecord[Core_EntityHelper.Employee.password] = hashedValue;
                employeeRecord[Core_EntityHelper.Employee.passwordFlag] = hashedValue;
                employeeRecord[Core_EntityHelper.Employee.salt] = Convert.ToBase64String(salt);

                _service.Update(employeeRecord);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EncryptAppKey(Models.Employee employee)
        {
            throw new NotImplementedException();
        }

        public bool EncryptAppSecret(Models.Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}


