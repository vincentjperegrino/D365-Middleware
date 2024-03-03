using KTI.Moo.Extensions.Shopify.Model;
using Microsoft.Extensions.Options;
using Xunit;


namespace TestShopify.Domain
{
    public class Product : Model.ShopifyBase
    {
        KTI.Moo.Extensions.Shopify.Domain.Product _domain;

        public Product()
        {
            _domain = new KTI.Moo.Extensions.Shopify.Domain.Product(TestConfig2);
        }

        [Fact]
        public void GetProduct_Success()
        {
            //assemble
            var productid = 8725989130559;

            //act
            var result = _domain.Get(productid);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
        }


        [Fact]
        public void DeleteProduct_Success()
        {

            try {

                //assemble
                var product = new KTI.Moo.Extensions.Shopify.Model.Product()
                {
                    id = 8728519639359,
                    body_html = "Updated in Test without product id in image",
                    title = "Updated Sample Product",
                    images = new()
                {
                    new()
                    {
                        //src = "https://ktimoostorage.blob.core.windows.net/ncci-dev/ab8f3917-accc-ed11-a7c7-002248ecf784_sample%20image.jpg?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D"
                        src =  "https://static.miraheze.org/hololivewiki/5/55/Ninomae_Ina%27nis_-_Full_Illustration_Mini.png"
                    }

                }

                };

                //act
                var result = _domain.Delete(product);

            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("Shopify Delete. (404 Not Found) Not Found"));

            }
          

            //assert
            //Assert.IsAssignableFrom<bool>(result);
        }



        [Fact]
        public void GetProduct_BySKU_Success()
        {
            //assemble
            var productSKU = "prod2";

            //act
            var result = _domain.Get(productSKU);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
        }

        [Fact]
        public void UpdateProduct_BySKU_Success()
        {
            //assemble
            var product = new KTI.Moo.Extensions.Shopify.Model.Product()
            {
                id = 8725989130559,
                body_html = "Updated in Test without product id in image",
                title = "Updated Sample Product",
                images = new()
                {
                    new()
                    {
                        //src = "https://ktimoostorage.blob.core.windows.net/ncci-dev/ab8f3917-accc-ed11-a7c7-002248ecf784_sample%20image.jpg?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D"
                        //src =  "https://static.miraheze.org/hololivewiki/5/55/Ninomae_Ina%27nis_-_Full_Illustration_Mini.png"
                        src = "https://ktimoostorage.blob.core.windows.net/ncci-dev/ab8f3917-accc-ed11-a7c7-002248ecf784_sample%20image.jpg?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D"
                    }

                }

            };

            //act
            var result = _domain.Update(product);



            //assert
            Assert.IsAssignableFrom<bool>(result);
        }

        [Fact]
        public void AddProduct_BySKU_Success()
        {
            //assemble
            var product = new KTI.Moo.Extensions.Shopify.Model.Product()
            {
                body_html = "New T-Shirt",
                title = "Added New Blue Product",
                handle = "test handle",
                images = new()
                {
                    new()
                    {

                        src = "https://kappa.ph/cdn/shop/files/KPMKN4TS94123-2-1-KappaSportsLogoTshirt-DrkBl-Web4_1200x.jpg?v=1694051544"

                    }
                },
                variants = new()
                {
                    new()
                    {
                        sku = "prod2",
                        inventory_quantity = 50
                    }
                }

            };

            //act
            var result = _domain.Add(product);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
        }





        [Fact]
        public void UpsertProduct_Success()
        {
            //assemble
            var product = new KTI.Moo.Extensions.Shopify.Model.Product()
            {
                id = 8929725677887,
                body_html = "Upsert Update in Test",
                title = "Upsert Update Sample Product",

                //images = new()
                //{
                //    new()
                //    {

                //        src = "https://ktimoostorage.blob.core.windows.net/ncci-dev/ab8f3917-accc-ed11-a7c7-002248ecf784_sample%20image.jpg?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D"

                //    }
                //},

                //variants = new()
                //{
                //    new()
                //    {
                //        sku = "prod2",
                //        inventory_quantity = 22,

                //        id = 46535748583743,
                //    }
                //},

                metafields = new()
                {
                    new()
                    {
                       //owner_id = 8929725677887,

                        value = "Turtle",
                        Namespace = "custom",
                        key = "design",



                    }
                }

            };

            //act
            var result = _domain.Upsert(product);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
        }





        [Fact]
        public void AddImage_Success()
        {
            //assemble
            var productid = 8725989130559;
            var externalSource = "https://ktimoostorage.blob.core.windows.net/ncci-dev/ab8f3917-accc-ed11-a7c7-002248ecf784_sample%20image.jpg?si=MooRead&spr=https&sv=2021-06-08&sr=c&sig=yKThooN0IrTIDohq5Sp8xuyDHZeCjdWMgesmERZF5pc%3D";
          //  var externalSource = "https://static.miraheze.org/hololivewiki/5/55/Ninomae_Ina%27nis_-_Full_Illustration_Mini.png";

            //act
            var result = _domain.AddImage(productid, externalSource, true);

            //assert
            Assert.IsAssignableFrom<bool>(result);
        }


        //[Fact]
        //public void RemoveImage_Success()
        //{
        //    //assemble
        //    var productid = 8725989130559;
        //    var index = 1;

        //    //act
        //    var result = _domain.RemoveImage(productid, index);

        //    //assert
        //    Assert.IsAssignableFrom<bool>(result);
        //}

        [Fact]
        public void SetAsPrimaryImage_Success()
        {
            //assemble
            var productid = 8725989130559;
            var index = 2;

            //act
            var result = _domain.SetPrimaryImage(productid, index);

            //assert
            Assert.IsAssignableFrom<bool>(result);
        }






        [Fact]
        public void AddProduct_FullDetails_Success()
        {
            //assemble
            var product = new KTI.Moo.Extensions.Shopify.Model.Product()
            {
                title = "Blue T-Shirt",
                body_html = "Order our brand new blue shirt",

                handle = "test handle",
                images = new()
                {
                    new()
                    {

                        src = "https://kappa.ph/cdn/shop/files/KPMKN4TS94123-2-1-KappaSportsLogoTshirt-DrkBl-Web4_1200x.jpg?v=1694051544"

                    }
                },

                variants = new()
                {
                    new()
                    {
                        sku = "prod2",
                        inventory_quantity = 50,
                        compare_at_price = 100,

                        price = 50,

                        requires_shipping = true,
                        weight = 130,
                        weight_unit = "g",
                        taxable = true,
                        option1 = "S",


                    },
                     new()
                     {
                        sku = "prod2",
                        option1 = "M",
                        price = 60,


                    },
                },

                options = new()
                {
                    new()
                    {

                        name = "Size",
                    }
                },


                metafields = new()
                {
                    new()
                    {


                        value = "Turtle",
                        Namespace = "custom",
                        key = "design",
 


                    }
                },

                tags = "blue",
                    
            };

            //act
            var result = _domain.Add(product);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
        }


        public class RandomNumberGenerator
        {
            private readonly Random random = new Random();

            public int GetNumber()
            {
                lock (random)
                {
                    return random.Next(10, 99);
                }
            }
        }

        [Fact]
        public async Task AddTenProductsAtTheSameTime_Success()
        {
            var numberGenerator = new RandomNumberGenerator();

            // Create an array of 50 products objects with unique data
            var products = Enumerable.Range(0, 20).Select(i => new KTI.Moo.Extensions.Shopify.Model.Product
            {
                title = $"Title: {numberGenerator.GetNumber()}",
                body_html = $"body html number:  {numberGenerator.GetNumber()}"
            }).ToArray();

            var tasks = products.Select(async product =>
            {
                // Retry logic with max of 3 attempts
                int maxRetries = 3;
                int currentAttempt = 0;
                while (currentAttempt < maxRetries)
                {
                    try
                    {
                        // Add product asynchronously
                        return await _domain.AddAsync(product);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as needed
                        currentAttempt++;
                    }
                }
                // Return null if all attempts fail
                return null;
            });

            // Wait for all tasks to complete asynchronously
            var results = await Task.WhenAll(tasks);

            // Assert
            foreach (var result in results)
            {
                // Check if the result is not null and has a valid id
                Assert.NotNull(result);
                Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Product>(result);
                Assert.True(result.id > 0);
            }
        }


    }
}