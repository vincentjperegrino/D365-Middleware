using Kti.Moo.Cyware.Model.DTO;
using KTI.Moo.Cyware.Helpers;
using KTI.Moo.Cyware.Model.DTO;
using KTI.Moo.Extensions.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model.DTO
{
    public class CustomerPOLL
    {
        [SortOrder(1)]
        [MaxLength(16)]
        public string CustomerId { get; set; }
        [SortOrder(2)]
        [MaxLength(3)]
        public string CurrencyCode { get; set; }
        [SortOrder(3)]
        [MaxLength(10)]
        public string LocationCode { get; set; }
        [SortOrder(4)]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [SortOrder(5)]
        [MaxLength(1)]
        public string MiddleName { get; set; }
        [SortOrder(6)]
        [MaxLength(30)]
        public string LastName { get; set; }
        [SortOrder(7)]
        [MaxLength(40)]
        public string CompanyName { get; set; }
        [SortOrder(8)]
        [MaxLength(128)]
        public string Remarks { get; set; }
        [SortOrder(9)]
        [MaxLength(30)]
        public string NickName { get; set; }
        [SortOrder(10)]
        [MaxLength(11)]
        public string BirthDay { get; set; }
        [SortOrder(11)]
        [MaxLength(128)]
        public string EmailAddress { get; set; }
        [SortOrder(12)]
        [MaxLength(32)]
        public string ContactNumber { get; set; }

        [SortOrder(13)]
        [MaxLength(20)]
        public string Type { get;  set; }
        [SortOrder(14)]
        [MaxLength(63)]
        public string Name { get; set; }
        [SortOrder(15)]
        [MaxLength(20)]
        public string CustomerGroup { get; set; }
        [SortOrder(16)]
        [MaxLength(20)]
        public string PriceGroup { get; set; }





        PollMapping helper = new PollMapping();

        public CustomerPOLL(KTI.Moo.Extensions.Cyware.Model.Customer cust)
        {
            this.CustomerId = helper.FormatStringAddSpacePadding(cust.CustomerId, (typeof(CustomerPOLL).GetProperty("CustomerId").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CurrencyCode = helper.FormatStringAddSpacePadding(cust.CurrencyCode, (typeof(CustomerPOLL).GetProperty("CurrencyCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.LocationCode = helper.FormatStringAddSpacePadding(cust.LocationCode, (typeof(CustomerPOLL).GetProperty("LocationCode").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.FirstName = helper.FormatStringAddSpacePadding(cust.firstname, (typeof(CustomerPOLL).GetProperty("FirstName").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.MiddleName = helper.FormatStringAddSpacePadding(cust.middlename, (typeof(CustomerPOLL).GetProperty("MiddleName").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.LastName = helper.FormatStringAddSpacePadding(cust.lastname, (typeof(CustomerPOLL).GetProperty("LastName").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CompanyName = helper.FormatStringAddSpacePadding(cust.CompanyName, (typeof(CustomerPOLL).GetProperty("CompanyName").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Remarks = helper.FormatStringAddSpacePadding(cust.Remarks, (typeof(CustomerPOLL).GetProperty("Remarks").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.NickName = helper.FormatStringAddSpacePadding(cust.nickname, (typeof(CustomerPOLL).GetProperty("NickName").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.BirthDay = helper.FormatStringAddSpacePadding(cust.birthdate, (typeof(CustomerPOLL).GetProperty("BirthDay").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.EmailAddress = helper.FormatStringAddSpacePadding(cust.PrimaryContactEmail, (typeof(CustomerPOLL).GetProperty("EmailAddress").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.ContactNumber = helper.FormatStringAddSpacePadding(cust.ContactNumber, (typeof(CustomerPOLL).GetProperty("ContactNumber").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Type = helper.FormatStringAddSpacePadding(cust.Type, (typeof(CustomerPOLL).GetProperty("Type").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.Name = helper.FormatStringAddSpacePadding(cust.Name, (typeof(CustomerPOLL).GetProperty("Name").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.CustomerGroup = helper.FormatStringAddSpacePadding(cust.CustomerGroup, (typeof(CustomerPOLL).GetProperty("CustomerGroup").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);
            this.PriceGroup = helper.FormatStringAddSpacePadding(cust.PriceGroup, (typeof(CustomerPOLL).GetProperty("PriceGroup").GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault() as MaxLengthAttribute)?.Length ?? 0);

        }

        public string Concat(CustomerPOLL customerPOLL)
        {
            return helper.ConcatenateValues(customerPOLL);
        }
    }
}
