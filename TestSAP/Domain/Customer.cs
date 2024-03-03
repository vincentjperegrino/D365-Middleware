using System.Collections.Generic;
using Xunit;

namespace TestSAP.Domain;

public class Customer : Model.SAPBase
{
    [Fact]
    public void GetCustomers_Success()
    {
        KTI.Moo.Extensions.SAP.Domain.Customer Domain = new(config);

        var SAPcardCode = "MAG02";

        var response = Domain.Get(SAPcardCode);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Customer>(response);

    }


    [Fact]
    public void AddCustomers_Success()
    {
        KTI.Moo.Extensions.SAP.Domain.Customer Domain = new(config);

        KTI.Moo.Extensions.SAP.Model.Customer Model = new()
        {
            kti_sapbpcode = "MAG02",
            CardName = "San Juan Nico",
            mobilephone = "09847293882",
            EmailAddress = "SanJuan@gmail.com",
            Channel = "Magento",
            MemberCode = "NC00984",

            Addresses = new()
            {
                new()
                {
                    RowNum = 0,
                    AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Billing,
                    address_line1 = "Manila Street",
                    address_postalcode = "2893",
                    address_city = "Manila City",
                    address_country = "PH"

                }

            },
        };

            

    var response = Domain.Add(Model);

    Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Customer>(response);

    }


    [Fact]
    public void UpsertCustomers_Success()
    {
        KTI.Moo.Extensions.SAP.Domain.Customer Domain = new(config);

        KTI.Moo.Extensions.SAP.Model.Customer Model = new()
        {
            //kti_sapcardcode = "MAG01",
            CardName = "San Juan Nico",
            mobilephone = "09847293880",
            EmailAddress = "SanJuan@gmail.com",
            Channel = "Magento",
            MemberCode = "NC00984",

            Addresses = new()
            {
                new()
                {
                    RowNum = 0,
                    AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Billing,
                    address_line1 = "Manila Street",
                    address_postalcode = "2893",
                    address_city = "Manila City",
                    address_country = "PH"

                }

            },
        };



        var response = Domain.Upsert(Model);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Customer>(response);

    }

    [Fact]
    public void UpdateCustomers_Success()
    {
        KTI.Moo.Extensions.SAP.Domain.Customer Domain = new(config);

        KTI.Moo.Extensions.SAP.Model.Customer Model = new()
        {
            kti_sapbpcode = "MAG01",
            CardName = "San Juan Nico 2",
            mobilephone = "09847293881",
            EmailAddress = "SanJuan2@gmail.com",
            Channel = "Magento",
            MemberCode = "NC00984s",

            Addresses = new()
            {
                new()
                {
                    RowNum = 0,
                    AddressType = KTI.Moo.Extensions.SAP.Helper.Customer.AddressType.Billing,
                    address_line1 = "Manila Street",
                    address_postalcode = "2893",
                    address_city = "Manila City",
                    address_country = "PH"

                }

            },
        };



        var response = Domain.Update(Model);

        Assert.True(response);

    }
}
