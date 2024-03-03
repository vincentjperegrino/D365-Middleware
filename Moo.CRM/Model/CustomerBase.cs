
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KTI.Moo.CRM.Model;

public partial class CustomerBase : Base.Model.CustomerBase, IComparable
{

    #region properties

    public string address1_addressid { get; set; }
    [Range(1, 4)]
    public int address1_addresstypecode { get; set; }
    [StringLength(80)]
    public string address1_city { get; set; }
    [StringLength(80)]
    public string address1_country { get; set; }
    [StringLength(50)]
    public string address1_county { get; set; }
    [StringLength(50)]
    public string address1_fax { get; set; }
    [Range(1, 2)]
    public int address1_freighttermscode { get; set; }
    [Range(-90, 90)]
    public double address1_latitude { get; set; }
    [StringLength(250)]
    public string address1_line1 { get; set; }
    [StringLength(250)]
    public string address1_line2 { get; set; }
    [StringLength(250)]
    public string address1_line3 { get; set; }
    [Range(-180, 180)]
    public double address1_longitude { get; set; }
    [StringLength(200)]
    public string address1_name { get; set; }
    [StringLength(20)]
    public string address1_postalcode { get; set; }
    [StringLength(100)]
    public string address1_postofficebox { get; set; }
    [StringLength(100)]
    public string address1_primarycontactname { get; set; }
    [Range(1, 7)]
    public int address1_shippingmethodcode { get; set; }
    [StringLength(50)]
    public string address1_stateorprovince { get; set; }
    [StringLength(50)]
    public string address1_telephone1 { get; set; }
    [StringLength(50)]
    public string address1_telephone2 { get; set; }
    [StringLength(50)]
    public string address1_telephone3 { get; set; }
    [StringLength(4)]
    public string address1_upszone { get; set; }
    [Range(-1500, 1500)]
    public int address1_utcoffset { get; set; }
    public string address2_addressid { get; set; }
    [Range(1, 1)]
    public string address2_addresstypecode { get; set; }
    [StringLength(80)]
    public string address2_city { get; set; }
    [StringLength(80)]
    public string address2_country { get; set; }
    [StringLength(50)]
    public string address2_county { get; set; }
    [StringLength(50)]
    public string address2_fax { get; set; }
    [Range(1, 1)]
    public int address2_freighttermscode { get; set; }
    [Range(-90, 90)]
    public double address2_latitude { get; set; }
    [StringLength(250)]
    public string address2_line1 { get; set; }
    [StringLength(250)]
    public string address2_line2 { get; set; }
    [StringLength(250)]
    public string address2_line3 { get; set; }
    [Range(-180, 180)]
    public double address2_longitude { get; set; }
    [StringLength(200)]
    public string address2_name { get; set; }
    [StringLength(20)]
    public string address2_postalcode { get; set; }
    [StringLength(100)]
    public string address2_postofficebox { get; set; }
    [StringLength(100)]
    public string address2_primarycontactname { get; set; }
    [Range(1, 1)]
    public int address2_shippingmethodcode { get; set; }
    [StringLength(50)]
    public string address2_stateorprovince { get; set; }
    [StringLength(50)]
    public string address2_telephone1 { get; set; }
    [StringLength(50)]
    public string address2_telephone2 { get; set; }
    [StringLength(50)]
    public string address2_telephone3 { get; set; }
    [StringLength(4)]
    public string address2_upszone { get; set; }
    [Range(-1500, 1500)]
    public int address2_utcoffset { get; set; }
    public string address3_addressid { get; set; }
    [Range(1, 1)]
    public string address3_addresstypecode { get; set; }
    [StringLength(80)]
    public string address3_city { get; set; }
    [StringLength(80)]
    public string address3_country { get; set; }
    [StringLength(50)]
    public string address3_county { get; set; }
    [StringLength(50)]
    public string address3_fax { get; set; }
    [Range(1, 1)]
    public int address3_freighttermscode { get; set; }
    [Range(-90, 90)]
    public double address3_latitude { get; set; }
    [StringLength(250)]
    public string address3_line1 { get; set; }
    [StringLength(250)]
    public string address3_line2 { get; set; }
    [StringLength(250)]
    public string address3_line3 { get; set; }
    [Range(-180, 180)]
    public double address3_longitude { get; set; }
    [StringLength(200)]
    public string address3_name { get; set; }
    [StringLength(20)]
    public string address3_postalcode { get; set; }
    [StringLength(100)]
    public string address3_postofficebox { get; set; }
    [StringLength(100)]
    public string address3_primarycontactname { get; set; }
    [Range(1, 1)]
    public int address3_shippingmethodcode { get; set; }
    [StringLength(50)]
    public string address3_stateorprovince { get; set; }
    [StringLength(50)]
    public string address3_telephone1 { get; set; }
    [StringLength(50)]
    public string address3_telephone2 { get; set; }
    [StringLength(50)]
    public string address3_telephone3 { get; set; }
    [StringLength(4)]
    public string address3_upszone { get; set; }
    [Range(-1500, 1500)]
    public int address3_utcoffset { get; set; }

    [StringLength(100)]
    public string emailaddress1 { get; set; }
    [StringLength(100)]
    public string emailaddress2 { get; set; }
    [StringLength(100)]
    public string emailaddress3 { get; set; }
    [StringLength(50)]
    public string telephone1 { get; set; }
    [StringLength(50)]
    public string telephone2 { get; set; }
    [StringLength(50)]
    public string telephone3 { get; set; }
    public int kti_socialchannelorigin { get; set; }
    public string kti_sourceid { get; set; }
    public int kti_customertype { get; set; }
    public string kti_magentoid { get; set; }
    public string kti_sapbpcode { get; set; }
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public int is_modified { get; set; }
    public DateTime createdon { get; set; }
    public DateTime modifiedon { get; set; }


    #endregion

    public virtual int CompareTo(object customerFromExtensions)
    {

        var customer = (CustomerBase)customerFromExtensions;


        if (!string.IsNullOrWhiteSpace(customer.firstname) && (this.firstname != customer.firstname))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.lastname) && (this.lastname != customer.lastname))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.emailaddress1) && (this.emailaddress1 != customer.emailaddress1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_line1) && (this.address1_line1 != customer.address1_line1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_line2) && (this.address1_line2 != customer.address1_line2))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_line3) && (this.address1_line3 != customer.address1_line3))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_city) && (this.address1_city != customer.address1_city))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_postalcode) && (this.address1_postalcode != customer.address1_postalcode))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_telephone1) && (this.address1_telephone1 != customer.address1_telephone1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_stateorprovince) && (this.address1_stateorprovince != customer.address1_stateorprovince))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.telephone1) && (this.telephone1 != customer.telephone1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address1_country) && (this.address1_country != customer.address1_country))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_line1) && (this.address2_line1 != customer.address2_line1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_line2) && (this.address2_line2 != customer.address2_line2))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_line3) && (this.address2_line3 != customer.address2_line3))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_line3) && (this.address2_line3 != customer.address2_line3))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_city) && (this.address2_city != customer.address2_city))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_postalcode) && (this.address2_postalcode != customer.address2_postalcode))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_stateorprovince) && (this.address2_stateorprovince != customer.address2_stateorprovince))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_country) && (this.address2_country != customer.address2_country))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.address2_telephone1) && (this.address2_telephone1 != customer.address2_telephone1))
        {
            return 1;
        }

        if (!string.IsNullOrWhiteSpace(customer.telephone2) && (this.telephone2 != customer.telephone2))
        {
            return 1;
        }

        return 0;
    }

}

