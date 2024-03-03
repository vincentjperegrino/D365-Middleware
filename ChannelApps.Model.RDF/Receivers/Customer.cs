using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.ChannelApps.Model.RDF.Receivers;

public class Customer : Moo.FO.Model.CustomerBase
{
    public string companyId { get; set; }
    public string domainType { get; set; }
    public string CustomerGroupId { get; set; }
    public Customer()
    {

    }
    public Customer(KTI.Moo.Extensions.Cyware.Model.Customer _customer)
    {
        //this.companyId = _customer.companyId;
        //this.domainType = _customer.domaintType;
        this.dataAreaId = _customer.dataAreaId;
        this.LanguageId = _customer.LanguageId;
        this.NameAlias = _customer.NameAlias;
        this.OrganizationName = _customer.OrganizationName;
        this.PersonFirstName = _customer.PersonFirstName;
        this.PersonLastName = _customer.PersonLastName;
        this.PersonMiddleName = _customer.PersonMiddleName;
        this.PersonPhoneticFirstName = _customer.PersonPhoneticFirstName;
        this.PersonPhoneticLastName = _customer.PersonPhoneticLastName;
        this.PersonPhoneticMiddleName = _customer.PersonPhoneticMiddlename;
        this.PrimaryContactEmail = _customer.PrimaryContactEmail;
        this.SalesCurrencyCode = _customer.SalesCurrencyCode; 
    }   
} 
