using KTI.Moo.Extensions.Shopify.Model.DTO;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Shopify.Model;

public class Customer : KTI.Moo.Extensions.Core.Model.CustomerBase
{
    //public bool AcceptsMarketing { get; set; }
    //public DateTime AcceptsMarketingUpdatedAt { get; set; }

    public Customer()
    {
    
    }

    public Customer(ShopifySharp.Customer customer)
    {
        
        Currency = customer.Currency;
        CreatedAt = customer.CreatedAt?.DateTime ?? default;


        
        if (customer.DefaultAddress != null)
        {
            DefaultAddress = new Model.Address(customer.DefaultAddress);
        }

        Email = customer.Email;
        FirstName = customer.FirstName;
        Id = customer.Id ?? default;
        LastName = customer.LastName;
        LastOrderId = customer.LastOrderId ?? default;
        LastOrderName = customer.LastOrderName;
        MultipassIdentifier = customer.MultipassIdentifier;
        Note = customer.Note;
        OrdersCount = customer.OrdersCount ?? default;
        //Password = customer.Password;
        //PasswordConfirmation = customer.PasswordConfirmation;
        Phone = customer.Phone;
        State = customer.State;
        Tags = customer.Tags;
        TaxExempt = customer.TaxExempt ?? default;
        TotalSpent = customer.TotalSpent ?? default;
        UpdatedAt = customer.UpdatedAt?.DateTime ?? default;
        VerifiedEmail = customer.VerifiedEmail ?? default;
        TaxExemptions = customer.TaxExemptions;
        if (customer.EmailMarketingConsent != null)
        {
            EmailMarketingConsent = new Model.EmailMarketingConsent(customer.EmailMarketingConsent);
        }

        if (customer.SmsMarketingConsent != null)
        {
            SmsMarketingConsent = new Model.SmsMarketingConsent(customer.SmsMarketingConsent);
        }
        
   

        Addresses = customer.Addresses?.Select(address => new Address(address)).ToList();
        Metafield = customer.Metafields?.Select(metaField => new MetaField(metaField)).ToList();



    }


    public List<Address> Addresses { get; set; }
    public EmailMarketingConsent EmailMarketingConsent { get; set; }

    public List<MetaField> Metafield { get; set; }
    public SmsMarketingConsent SmsMarketingConsent { get; set; }

    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    public Address DefaultAddress { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public override string firstname { get => FirstName; set => FirstName = value; }
    public long Id { get; set; }
    public string LastName { get; set; }
    public override string lastname { get => LastName; set => LastName = value; }
    public long LastOrderId { get; set; }
    public string LastOrderName { get; set; }
    public string MarketingOptInLevel { get; set; }
    public string MultipassIdentifier { get; set; }
    public string Note { get; set; }
    public int OrdersCount { get; set; }
    //public string Password { get; set; }
    //public string PasswordConfirmation { get; set; }
    public string Phone { get; set; }
    public string State { get; set; }
    public string Tags { get; set; }
    public bool TaxExempt { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool VerifiedEmail { get; set; }
    public string[] TaxExemptions { get; set; }


    





}
