using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.Model
{
    public class ForExBase
    {
        public string RateTypeName { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public DateTime StartDate { get; set; }

        public decimal Rate { get; set; }

        public DateTime EndDate { get; set; }

        public string ConversionFactor { get; set; }

        public string RateTypeDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string moosourcesystem { get; set; }
        //Customized
        [DataType(DataType.Text)]
        public string mooexternalid { get; set; }
        //Customized
        //[CompanyIdAttribute]
        public int companyid { get; set; }
    }
}
