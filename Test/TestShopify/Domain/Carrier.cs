using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestShopify.Domain
{
    public class Carrier : Model.ShopifyBase
    {
        KTI.Moo.Extensions.Shopify.Domain.Carrier _domain;


        public Carrier()
        {
            _domain = new KTI.Moo.Extensions.Shopify.Domain.Carrier(TestConfig2);
        }

        //[Fact]
        //public void GetCarrier_Success()
        //{

        //    try
        //    {
        //        //assemble
        //        var carrierid = 83313099071;


        //        //act
        //        var result = _domain.Get(carrierid);

        //        //assert
        //        Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Carrier>(result);



        //    }

        //    catch (Exception ex)

        //    {
        //        Assert.True(ex.Message.Contains("404 Not Found"));
        //    }   

           
        //}





        //[Fact]
        //public void DeleteCarrier_Success()
        //{
        //    try
        //    {
        //        var carrier = new KTI.Moo.Extensions.Shopify.Model.Carrier()
        //        {
        //            id = 83335529771

        //        };

        //        //act
        //        var result = _domain.Delete(carrier);

        //    }

        //    catch (Exception ex)

        //    {
        //        Assert.True(ex.Message.Contains("error: Not Found"));

        //    }
        //    //assemble


        //    ////assert
        //    //Assert.IsAssignableFrom<bool>(result);
        //}


        //[Fact]
        //public void UpdateCarrier_Success()
        //{

        //    try {

        //        //assemble
        //        var carrier = new KTI.Moo.Extensions.Shopify.Model.Carrier()
        //        {
        //            id = 83316867391,
        //            name = "Jame's Updated Carrier",
        //            service_discovery = false,



        //    };

        //        //act
        //        var result = _domain.Update(carrier);



        //    }

        //    catch (Exception ex)
        //    {
        //        Assert.True(ex.Message.Length > 0);

        //    }
     

        //    //assert
        //    //Assert.IsAssignableFrom<bool>(result);
        //}


        //[Fact]
        //public void AddCarrier_Success()
        //{

        //    try
        //    {

        //        // Assemble
        //        var carrier = new KTI.Moo.Extensions.Shopify.Model.Carrier()
        //        {


        //            name = "Jame's VS Carrier",

        //            callback_url = "http://james.example.com/",



        //        };

        //        // Act
        //        var result = _domain.Add(carrier);

        //    }

        //    catch (Exception ex)
        //    {
        //        //Assert.True(ex.Message.Contains("Error: Carrier been configured"));
        //        Assert.True(ex.Message.Length > 0);

        //    }
          

        //    // Assert
        //   // Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Carrier>(result);
            
        //}


        //[Fact]
        //public void UpsertCarrier_Success()
        //{

        //    try
        //    {
        //        //assemble
        //        var carrier = new KTI.Moo.Extensions.Shopify.Model.Carrier()
        //        {
        //            id = 83313099071,
        //            name = "James upsert carrier"
        //        };
        //        //act
        //        var result = _domain.Upsert(carrier);


        //        Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Carrier>(result);
        //    }
        //    catch(Exception ex)
        //    {
        //        //Console.WriteLine(ex.Message);
        //        Assert.True(ex.Message.Length > 0);
        //    }
        //}






    }
}
