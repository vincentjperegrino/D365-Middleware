using KTI.Moo.Extensions.Cyware.App.Receiver.Models;

namespace KTI.Moo.FO.Model.DTO
{
    public class CustomerDTO : CustomerBase
    {
        public CustomerDTO(OrdersTransaction orderTransaction)
        {
            this.PersonFirstName = orderTransaction.Header.CustomerName;
            this.PersonMiddleName = "";
            this.PersonLastName = "";
            this.PersonPhoneticFirstName = orderTransaction.Header.CustomerName;
            this.PersonPhoneticMiddleName = "";
            this.PersonPhoneticLastName = "";
            this.PrimaryContactEmail = "";
            this.PrimaryContactPhone = "";
            this.OrganizationName = orderTransaction.Header.CustomerName;
            this.NameAlias = orderTransaction.Header.CustomerName;
            this.dataAreaId = "SRDF";
            this.CustomerGroupId = "ARTLYLTYCU";
            this.LanguageId = "en-US";
            this.SalesCurrencyCode = "PHP";
        }
        public CustomerDTO()
        { 
        }

        //public CustomerDTO(OrdersTransaction orderTransaction)
        //{
        //    this.PersonFirstName = orderTransaction.Header.CustomerFirstName;
        //    this.PersonMiddleName = orderTransaction.Header.CustomerMiddleName;
        //    this.PersonLastName = orderTransaction.Header.CustomerLastName;
        //    this.PersonPhoneticFirstName = orderTransaction.Header.CustomerFirstName;
        //    this.PersonPhoneticMiddleName = orderTransaction.Header.CustomerMiddleName;
        //    this.PersonPhoneticLastName = orderTransaction.Header.CustomerLastName;
        //    this.PrimaryContactEmail = orderTransaction.Header.CustomerEmail;
        //    this.PrimaryContactPhone = orderTransaction.Header.CustomerContactNumber;
        //    this.OrganizationName = !String.IsNullOrEmpty(orderTransaction.Header.MembershipNumber) ? orderTransaction.Header.MembershipNumber : orderTransaction.Header.CustomerFirstName + " " + orderTransaction.Header.CustomerMiddleName + " " + orderTransaction.Header.CustomerLastName;
        //    this.NameAlias = orderTransaction.Header.CustomerFirstName + " " + orderTransaction.Header.CustomerMiddleName + " " + orderTransaction.Header.CustomerLastName;
        //    this.dataAreaId = "SRDF";
        //    this.CustomerGroupId = "ARTLYLTYCU";
        //    this.LanguageId = "en-US";
        //    this.SalesCurrencyCode = "PHP";
        //}
    }
}
