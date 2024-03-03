using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.Extensions.Core.Model
{
    public class OrderBase
    {
        public OrderBase()
        {
        }

        public virtual int kti_socialchannelorigin { get; set; }
        [DataType(DataType.Text)]
        [JsonIgnore]
        public virtual string moosourcesystem { get; set; }
        public virtual string kti_channelurl { get; set; }

        public virtual string kti_sourceid { get; set; }
        public virtual string kti_couponcode { get; set; }

        public virtual string kti_socialchannel { get; set; }


        public virtual int companyid { get; set; }

        [Required]
        [StringLength(300)]
        public virtual string name { get; set; }
        [StringLength(80)]
        public virtual string billto_city { get; set; }
        [StringLength(150)]
        public virtual string billto_contactname { get; set; }
        [StringLength(80)]
        public virtual string billto_country { get; set; }
        [StringLength(50)]
        public virtual string billto_fax { get; set; }
        [StringLength(250)]
        public virtual string billto_line1 { get; set; }
        [StringLength(250)]
        public virtual string billto_line2 { get; set; }
        [StringLength(250)]
        public virtual string billto_line3 { get; set; }
        [StringLength(200)]
        public virtual string billto_name { get; set; }
        [StringLength(20)]
        public virtual string billto_postalcode { get; set; }
        [StringLength(50)]
        public virtual string billto_stateorprovince { get; set; }
        [StringLength(50)]
        public virtual string billto_telephone { get; set; }
        [Required]
        [JsonProperty("customerid_contact@odata.bind")]
        public virtual string customerid { get; set; }
        public virtual DateTime datefulfilled { get; set; }
        [StringLength(2000)]
        public virtual string description { get; set; }
        [Range(0, 1000000000000)]
        public virtual decimal discountamount { get; set; }
        [Range(0, 100)]
        public virtual decimal discountpercentage { get; set; }
        [StringLength(100)]
        public virtual string emailaddress { get; set; }
        [Range(0, 1000000000000)]
        public virtual decimal freightamount { get; set; }
        [Range(0, 2)]
        public virtual int freighttermscode { get; set; }
        public virtual bool ispricelocked { get; set; }
        public virtual DateTime lastbackofficesubmit { get; set; }
        public virtual DateTime lastonholdtime { get; set; }

        public virtual string kti_latitude1 { get; set; }
        public virtual string kti_longitude1 { get; set; }
        public virtual int kti_deliverymethod { get; set; }
        public virtual int kti_paymenttermscode { get; set; }
        public virtual bool kti_giftwrap { get; set; }
        public virtual string kti_gifttagmessage { get; set; }
        public virtual int kti_orderstatus { get; set; }
        [Range(0, 4)]
        public virtual int paymenttermscode { get; set; }
        [Required]
        [JsonProperty("pricelevelid@odata.bind")]
        public virtual string pricelevelid { get; set; }
        public virtual string salesorderid { get; set; }
        [Range(0, 7)]
        public virtual int shippingmethodcode { get; set; }
        [StringLength(80)]
        public virtual string shipto_city { get; set; }
        [StringLength(150)]
        public virtual string shipto_contactname { get; set; }
        [StringLength(80)]
        public virtual string shipto_country { get; set; }
        [StringLength(50)]
        public virtual string shipto_fax { get; set; }
        [Range(0, 1)]
        public virtual int shipto_freighttermscode { get; set; }
        [StringLength(250)]
        public virtual string shipto_line1 { get; set; }
        [StringLength(250)]
        public virtual string shipto_line2 { get; set; }
        [StringLength(250)]
        public virtual string shipto_line3 { get; set; }
        [StringLength(200)]
        public virtual string shipto_name { get; set; }
        [StringLength(20)]
        public virtual string shipto_postalcode { get; set; }
        [StringLength(50)]
        public virtual string shipto_stateorprovince { get; set; }
        [StringLength(50)]
        public virtual string shipto_telephone { get; set; }
        [Range(0, 4)]
        public virtual int statecode { get; set; }
        [Range(0, 690970000)]
        public virtual int statuscode { get; set; }
        public virtual DateTime submitdate { get; set; }
        [Range(0, 1000000000)]
        public virtual int submitstatus { get; set; }
        [StringLength(2000)]
        public virtual string submitstatusdescription { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totalamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totalamountlessfreight { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totaldiscountamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totallineitemamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totallineitemdiscountamount { get; set; }
        [Range(-922337203685477, 922337203685477)]
        public virtual decimal totaltax { get; set; }
        [JsonProperty("kti_BranchAssigned@odata.bind")]
        public virtual string branch_assigned { get; set; }
        public virtual string order_status { get; set; }
        [DataType(DataType.Text)]
        [JsonIgnore]
        public virtual string mooexternalid { get; set; }

        public virtual int kti_cancel_reason { get; set; }
        public virtual DateTime overriddencreatedon { get; set; }
    }


}
