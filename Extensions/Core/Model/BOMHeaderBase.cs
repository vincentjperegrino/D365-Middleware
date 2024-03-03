using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class BOMHeaderBase
    {
        public string BOMID { get; set; }
        public string MANUFACTUREDITEMNUMBER { get; set; }
        public string PRODUCTCOLORID { get; set; }
        public string PRODUCTIONSITEID { get; set; }
        public string PRODUCTCONFIGURATIONID { get; set; }
        public string PRODUCTSIZEID { get; set; }
        public string PRODUCTSTYLEID { get; set; }
        public string PRODUCTVERSIONID { get; set; }
        public string ISACTIVE { get; set; }
        public decimal FROMQUANTITY { get; set; }
        public DateTime VALIDFROMDATE { get; set; }
        public string APPROVERPERSONNELNUMBER { get; set; }
        public string BOMNAME { get; set; }
        public string ISAPPROVED { get; set; }

    }
}
