using Microsoft.Extensions.Options;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KTI.Moo.Extensions.Shopify.Model.DTO
{
    public class Customer : ShopifySharp.Customer
    {
        public Customer()
        {

        }

        public Customer(Model.Customer customer)
        {
            Currency = customer.Currency;
            if (customer.CreatedAt != default)
            {
                CreatedAt = customer.CreatedAt;
            }
            

            if (customer.DefaultAddress != null)
            {
                DefaultAddress = new Model.DTO.Address(customer.DefaultAddress);
            }

            Email = customer.Email;
            FirstName = customer.FirstName;
            Id = customer.Id;
            LastName = customer.LastName;
            LastOrderId = customer.LastOrderId;
            LastOrderName = customer.LastOrderName;
            //MarketingOptInLevel = customer.MarketingOptInLevel;
            MultipassIdentifier = customer.MultipassIdentifier;
            Note = customer.Note;
            OrdersCount = customer.OrdersCount;
            //Password = customer.Password;
            //PasswordConfirmation = customer.PasswordConfirmation;
            Phone = customer.Phone;
            State = customer.State;
            Tags = customer.Tags;
            TaxExempt = customer.TaxExempt;
            TotalSpent = customer.TotalSpent;
            VerifiedEmail = customer.VerifiedEmail;

            if (customer.EmailMarketingConsent != null)
            {
                EmailMarketingConsent = new Model.DTO.EmailMarketingConsent(customer.EmailMarketingConsent);
            }

            if (customer.SmsMarketingConsent != null)
            {
                SmsMarketingConsent = new Model.DTO.SmsMarketingConsent(customer.SmsMarketingConsent);
            }



            

            Addresses = customer.Addresses?.Select(address => new Address(address));
            Metafields = customer.Metafield?.Select(metaField => new MetaField(metaField));




        }


        //public string PasswordConfirmation { get; private set; }
        //public string MarketingOptInLevel { get; set; }
        //public string Password { get; set; }

    }
}
