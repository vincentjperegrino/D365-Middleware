using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class AddressBase
    {
        //public virtual List<TelephoneBase> AddressTelephone { get; set; }

        public virtual string first_name { get; set; }
        public virtual string last_name { get; set; }

        public virtual int address_id { get; set; }
        [Range(1, 4)]
        public virtual int address_addresstypecode { get; set; }
        [StringLength(80)]
        public virtual string address_city { get; set; }
        [StringLength(80)]
        public virtual string address_country { get; set; }
        [StringLength(50)]
        public virtual string address_county { get; set; }
        [StringLength(50)]
        public virtual string address_fax { get; set; }
        [Range(1, 2)]
        public virtual int address_freighttermscode { get; set; }
        [Range(-90, 90)]
        public virtual double address_latitude { get; set; }
        [StringLength(250)]
        public virtual string address_line1 { get; set; }
        [StringLength(250)]
        public virtual string address_line2 { get; set; }
        [StringLength(250)]
        public virtual string address_line3 { get; set; }
        [Range(-180, 180)]
        public virtual double address_longitude { get; set; }
        [StringLength(200)]
        public virtual string address_name { get; set; }
        [StringLength(20)]
        public virtual string address_postalcode { get; set; }
        [StringLength(100)]
        public virtual string address_postofficebox { get; set; }
        [StringLength(100)]
        public virtual string address_primarycontactname { get; set; }
        [Range(1, 7)]
        public virtual int address_shippingmethodcode { get; set; }
        [StringLength(50)]
        public virtual string address_stateorprovince { get; set; }

        [StringLength(4)]
        public virtual string address_upszone { get; set; }
        [Range(-1500, 1500)]
        public virtual int address_utcoffset { get; set; }


    }
}
