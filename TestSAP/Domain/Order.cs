using System;
using System.Collections.Generic;
using Xunit;

namespace TestSAP.Domain;

public class Order : Model.SAPBase
{
    private readonly KTI.Moo.Extensions.SAP.Domain.Order Domain;

    public Order()
    {
        Domain = new(config);
    }

    [Fact]
    public void GetOrders_Success()
    {

        var DocEntry = "78138";

        var response = Domain.Get(DocEntry);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Order>(response);

    }


    [Fact]
    public void GetOrdersbyField_Success()
    {

        var response = Domain.GetByField("U_WebOrderNo", "000011684");

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Order>(response);

    }


    [Fact]
    public void AddOrders_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Order Model = new()
        {
            DocNum = 123,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            OrderItems = new()
            {
                new()
                {
                    productid = "MAC0020",
                    quantity = 1,
                    priceperunit = 50
                }


            }



        };

        var response = Domain.Add(Model);

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Order>(response);

    }


    [Fact]
    public void UpdateOrders_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Order Model = new()
        {
            DocEntry = 78148,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            OrderItems = new()
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

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Order>(response);

    }

    [Fact]
    public void UpsertOrders_Success()
    {

        KTI.Moo.Extensions.SAP.Model.Order Model = new()
        {
            //DocEntry = 78148,
            DocDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            DocDueDate = DateTime.Now.ToString(KTI.Moo.Extensions.SAP.Helper.Date.DocDates),
            customerid = "MAG02",
            description = "Posted by KTI",
            Channel = "Magento",
            OrderItems = new()
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

        Assert.IsAssignableFrom<KTI.Moo.Extensions.SAP.Model.Order>(response);

    }






}
