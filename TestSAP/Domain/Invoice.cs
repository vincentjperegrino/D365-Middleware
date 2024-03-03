
using System;
using System.Collections.Generic;
using Xunit;

namespace TestSAP.Domain;

public class Invoice : Model.SAPBase
{
    private readonly KTI.Moo.Extensions.SAP.Domain.Invoice Domain;

    public Invoice()
    {
        Domain = new(config);
    }

    [Fact]
    public void GetInvoice_Success()
    {

        var DocEntry = "1";

        var response = Domain.Get(DocEntry);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Invoice>(response);

    }

    [Fact]
    public void AddInvoice_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Invoice Model = new()
        {
           // DocNum = 123,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            InvoiceItems = new()
            {
                new KTI.Moo.Extensions.SAP.Model.InvoiceItem()
                {
                    productid = "COFB052",
                    //description = "MAC0020",
                    quantity = 1,
                    priceperunit = 50
                }


            }



        };

        var response = Domain.Add(Model);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Invoice>(response);

    }


    [Fact]
    public void UpdateInvoice_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Invoice Model = new()
        {
            DocEntry = 78148,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            InvoiceItems = new()
            {
                new()
                {
                    LineNum = 0,
                    productid = "MAC0020",
                    quantity = 1,
                    priceperunit = 50
                }
            }
        };

        var response = Domain.Update(Model);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Invoice>(response);

    }

    [Fact]
    public void UpsertInvoice_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Invoice Model = new()
        {
            //DocEntry = 78148,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            InvoiceItems = new()
            {
                new()
                {
                    LineNum = 0,
                    productid = "MAC0020",
                    quantity = 1,
                    priceperunit = 50,
                },
                new()
                {
                    LineNum = 1,
                    productid = "MAC0020",
                    quantity = 1,
                    priceperunit = 50,
                }
            }
        };

        var response = Domain.Upsert(Model);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Invoice>(response);

    }


}
