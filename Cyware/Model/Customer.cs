using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Cyware.Model
{
    public class Customer : Core.Model.CustomerBase
    {
        public string companyId { get; set; }
        public string domaintType { get; set; }
        public string dataAreaId { get; set; }
        public string LanguageId { get; set; }
        public string NameAlias { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonLastNamePrefix { get; set; }
        public string PersonMiddleName { get; set; }
        public string PersonPhoneticFirstName { get; set; }
        public string PersonPhoneticLastName { get; set; }
        public string PersonPhoneticMiddlename { get; set; }    
        public string PrimaryContactEmail { get; set; }
        public string SalesCurrencyCode { get; set; }
        public string OrganizationName { get; set; }

        /// <summary>
        /// /Customized for JOEL project
        /// </summary>

        public string CustomerId { get; set; }
        public string CurrencyCode { get; set; }
        public string LocationCode { get; set; }
        public string CompanyName { get; set; }
        public string Remarks { get; set; }
        public string ContactNumber { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }
        public string CustomerGroup { get; set; }
        public string PriceGroup { get; set; }



    }
}
