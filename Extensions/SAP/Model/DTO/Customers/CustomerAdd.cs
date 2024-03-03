
namespace KTI.Moo.Extensions.SAP.Model.DTO.Customers;
public class Upsert : Customer
{
    [JsonProperty("Address")]
    public string Address
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Billing))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Billing)
                                     .FirstOrDefault().address_line1;
            }

            return default;

        }
    }

    [JsonProperty("ZipCode")]
    public string ZipCode
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Billing))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Billing)
                                     .FirstOrDefault().address_postalcode;
            }
            return default;
        }
    }


    [JsonProperty("City")]
    public string City
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Billing))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Billing)
                                     .FirstOrDefault().address_city;
            }
            return default;
        }
    }


    [JsonProperty("Country")]
    public string Country
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Billing))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Billing)
                                     .FirstOrDefault().address_country;
            }
            return default;
        }
    }


    [JsonProperty("MailAddress")]
    public string MailAddress
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Shipping))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Shipping)
                                     .FirstOrDefault().address_line1;
            }
            return default;
        }
    }


    [JsonProperty("MailZipCode")]
    public string MailZipCode
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Shipping))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Shipping)
                                     .FirstOrDefault().address_postalcode;
            }
            return default;
        }
    }

    [JsonProperty("MailCity")]
    public string MailCity
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Shipping))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Shipping)
                                     .FirstOrDefault().address_city;
            }
            return default;
        }
    }

    [JsonProperty("MailCountry")]
    public string MailCountry
    {
        get
        {
            if (this.Addresses is null || this.Addresses.Count <= 0)
            {
                return default;
            }

            if (this.Addresses.Any(address => address.AddressType == Helper.Customer.AddressType.Shipping))
            {
                return this.Addresses.Where(address => address.AddressType == Helper.Customer.AddressType.Shipping)
                                     .FirstOrDefault().address_country;
            }

            return default;
        }
    }


    //Ignore in DTO
    [JsonProperty("BPAddresses")]
    [JsonIgnore]
    public override List<Address> Addresses { get; set; }
}
