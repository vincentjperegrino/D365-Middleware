using ShopifySharp.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;



namespace TestShopify.Domain
{
    [Collection("MyTests2")]
    public class Customer : Model.ShopifyBase
    {
        KTI.Moo.Extensions.Shopify.Domain.Customer _domain;


        public Customer()
        {
            _domain = new KTI.Moo.Extensions.Shopify.Domain.Customer(TestConfig2);
        }

        [Fact]
        public void GetCustomer_Success()
        {
            //assemble
            var firstname = "jim";


            //act
            var result = _domain.Get(firstname);


            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        }

        [Fact]
        public void GetCustomerEmailSuccess()
        {
            //assemble
            var Email = "jame222.test@test.com";


            //act
            var result = _domain.GetByEmail(Email);


            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        }

        //[Fact]
        //public void GetCustomerListSuccess()
        //{
        //    //assemble
        //    var Email = "james2.test@test.com";


        //    //act
        //    var result = _domain.GetList(Email);


        //    //assert
        //    Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        //}




        //[Fact]
        //public void DeleteCustomer_Success()
        //{
        //    //assemble
        //    var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
        //    {
        //        Id = 7321355977023

        //    };

        //    //act
        //    var result = _domain.Delete(customer);

        //    //assert
        //    Assert.IsAssignableFrom<bool>(result);
        //}


        [Fact]
        public void UpdateCustomer_Success()
        {
            //assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Id = 7300699980095,



            };

            //act
            var result = _domain.Update(customer);



            //assert
            Assert.IsAssignableFrom<bool>(result);
        }


        // Add Customer with Wrong email format
        [Fact]
        public void AddCustomerWithWrongEmailFormat_Fail()
        {
            try
            {
                // Assemble
                var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
                {

                    Currency = "JPY",
                    CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                    Email = "bob.normanffafsa.com",
                    Phone = "+12345671133",
                    FirstName = "John",
                    LastName = "Norman",
                    LastOrderId = 234132602919,
                    LastOrderName = "#1169",
                    MultipassIdentifier = null,
                    Note = "Placed an order that had a fraud warning",
                    OrdersCount = 3,
                    State = "disabled",
                    Tags = "loyal",
                    TaxExempt = true,
                    TotalSpent = 375.30M,
                    UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                    VerifiedEmail = true,
                    TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

                    Addresses = new()
                {
                    new()
                    {

                        FirstName = "Bob",
                        LastName = "Norman",
                        Address1 = "Chestnut Street 92",
                        Address2 = "Apartment 2",
                        City = "Louisville",
                        Province = "Kentucky",
                        Country = "United States",
                        Zip = "40202",
                        Phone = "555-625-1199",
                        ProvinceCode = "KY",
                        CountryCode = "US",
                        CountryName = "United States",
                        Default = true
                    }

                }



                };

                // Act
                var result = _domain.Add(customer);
            }

            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("email: is invalid"));
            }


            // Assert
            ////Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);

        }

        // Add Customer with US Phone number
        [Fact]
        public void AddCustomerWithUSPhoneNumber_Success()
        {
            // Assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Currency = "JPY",
                CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                Email = "bob.norman@mail.example.com",
                Phone = "+12345625112",
                FirstName = "John",
                LastName = "Norman",
                LastOrderId = 234132602919,
                LastOrderName = "#1169",
                MultipassIdentifier = null,
                Note = "Placed an order that had a fraud warning",
                OrdersCount = 3,
                State = "disabled",
                Tags = "loyal",
                TaxExempt = true,
                TotalSpent = 375.30M,
                UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                VerifiedEmail = true,
                TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

                Addresses = new()
                {
                    new()
                    {

                        FirstName = "Bob",
                        LastName = "Norman",
                        Address1 = "Chestnut Street 92",
                        Address2 = "Apartment 2",
                        City = "Louisville",
                        Province = "Kentucky",
                        Country = "United States",
                        Zip = "40202",
                        Phone = "555-625-1199",
                        ProvinceCode = "KY",
                        CountryCode = "US",
                        CountryName = "United States",
                        Default = true
                    }

                }



            };

            // Act
            var result = _domain.Add(customer);

            // Assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
            Assert.True(result.Id > 0);
        }

        //// Add Customer with Same Phone number
        //[Fact]
        //public void AddCustomerWithUSPhoneNumber_Fail()
        //{
        //    // Assemble
        //    var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
        //    {
        //        Currency = "JPY",
        //        CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
        //        Email = "bob.norman@mail.example.com",
        //        Phone = "+12345676112",
        //        FirstName = "John",
        //        LastName = "Norman",
        //        LastOrderId = 234132602919,
        //        LastOrderName = "#1169",
        //        MultipassIdentifier = null,
        //        Note = "Placed an order that had a fraud warning",
        //        OrdersCount = 3,
        //        State = "disabled",
        //        Tags = "loyal",
        //        TaxExempt = true,
        //        TotalSpent = 375.30M,
        //        UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
        //        VerifiedEmail = true,
        //        TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

        //        Addresses = new()
        //        {
        //            new()
        //            {

        //                FirstName = "Bob",
        //                LastName = "Norman",
        //                Address1 = "Chestnut Street 92",
        //                Address2 = "Apartment 2",
        //                City = "Louisville",
        //                Province = "Kentucky",
        //                Country = "United States",
        //                Zip = "40202",
        //                Phone = "555-625-1199",
        //                ProvinceCode = "KY",
        //                CountryCode = "US",
        //                CountryName = "United States",
        //                Default = true
        //            }

        //        }



        //    };

        //    // Act
        //    var result = _domain.Add(customer);

        //    // Assert
        //    Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        //    Assert.True(result.Id > 0);
        //}


        //Add customer with 3000 characters for address field
        [Fact]
        public void AddCustomerWith3000CharactersForAddressField_Fail()
        {
            try
            {
                // Assemble
                var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
                {
                    Currency = "JPY",
                    CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                    Email = "bob.norman@mail.example.com",
                    Phone = "+12345676112",
                    FirstName = "John",
                    LastName = "Norman",
                    LastOrderId = 234132602919,
                    LastOrderName = "#1169",
                    MultipassIdentifier = null,
                    Note = "Placed an order that had a fraud warning",
                    OrdersCount = 3,
                    State = "disabled",
                    Tags = "loyal",
                    TaxExempt = true,
                    TotalSpent = 375.30M,
                    UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                    VerifiedEmail = true,
                    TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

                    Addresses = new()
                {
                    new()
                    {

                        FirstName = "Bob",
                        LastName = "Norman",
                        Address1 = "Chestnut Street Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\nChestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\nChestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\nChestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\nChestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\nChestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92Chestnut Street 92 C\r\n\r\n92 Chestnut Street 92 Chestnut Street 92 Chestnut Street 92 ChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnutChestnu Street 92",
                        Address2 = "Apartment partment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment2Apartment 2Apartment 2",
                        City = "Louisville LouisvilleLouisville",
                        Province = "Kentucky KentuckyKentucky",
                        Country = "United States United StatesUnited States",
                        Zip = "40202",
                        Phone = "555-625-1199",
                        ProvinceCode = "KY",
                        CountryCode = "US",
                        CountryName = "United States",
                        Default = true
                    }

                }



                };

                // Act
                var result = _domain.Add(customer);
            }
            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("is too long (maximum is 255 characters)"));
            }

            // Assert
            //Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        }


        // Add Customer with Firstname all special characters
        [Fact]
        public void AddCustomerWithFirstNameAllSpecialCharacters_Success()
        {
            // Assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Currency = "JPY",
                CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                Email = "bob.specialcharacters@mail.example.com",
                Phone = "+639292228877",
                FirstName = "!@#$%^&*()",
                LastName = "Norman",
                LastOrderId = 234132602919,
                LastOrderName = "#1169",
                MultipassIdentifier = null,
                Note = "Placed an order that had a fraud warning",
                OrdersCount = 3,
                State = "disabled",
                Tags = "loyal",
                TaxExempt = true,
                TotalSpent = 375.30M,
                UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                VerifiedEmail = true,
                TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },



            };

            // Act
            var result = _domain.Add(customer);

            // Assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
            Assert.True(result.Id > 0);
        }



        // Add Customer with Lastname all special characters
        [Fact]
        public void AddCustomerWithLastNameAllSpecialCharacters_Success()
        {

            try
            {
                // Assemble
                var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
                {
                    Currency = "JPY",
                    CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                    Email = "bob.norman@mail.example.com",
                    Phone = "+12374671112",
                    FirstName = "Test",
                    LastName = "!@#$%^&*()",
                    LastOrderId = 234132602919,
                    LastOrderName = "#1169",
                    MultipassIdentifier = null,
                    Note = "Placed an order that had a fraud warning",
                    OrdersCount = 3,
                    State = "disabled",
                    Tags = "loyal",
                    TaxExempt = true,
                    TotalSpent = 375.30M,
                    UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                    VerifiedEmail = true,
                    TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },



                };

                // Act
                var result = _domain.Add(customer);

            }

            catch (Exception ex)
            {
                Assert.True(ex.Message.Contains("email: has already been taken"));
            }


            // Assert
            //Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);

        }







        // Add Complete Data
        [Fact]
        public void AddCustomer_Success()
        {
            // Assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Currency = "JPY",
                CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                Email = "bob.normssan@mail.example.com",
                Phone = "+12184671112",
                FirstName = "John",
                LastName = "Norman",
                LastOrderId = 234132602919,
                LastOrderName = "#1169",
                MultipassIdentifier = null,
                Note = "Placed an order that had a fraud warning",
                OrdersCount = 3,
                State = "disabled",
                Tags = "loyal",
                TaxExempt = true,
                TotalSpent = 375.30M,
                UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                VerifiedEmail = true,
                TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

                Addresses = new()
                {
                    new()
                    {

                        FirstName = "Bob",
                        LastName = "Norman",
                        Address1 = "Chestnut Street 92",
                        Address2 = "Apartment 2",
                        City = "Louisville",
                        Province = "Kentucky",
                        Country = "United States",
                        Zip = "40202",
                        Phone = "555-625-1199",
                        ProvinceCode = "KY",
                        CountryCode = "US",
                        CountryName = "United States",
                        Default = true
                    }

                }



            };

            // Act
            var result = _domain.Add(customer);

            // Assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
            Assert.True(result.Id > 0);
        }




        // Add Customer with No Lastname
        [Fact]
        public void AddCustomerWithNoLastname_Success()
        {
            // Assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Currency = "JPY",
                CreatedAt = DateTime.Parse("2013-06-27T08:48:27-04:00"),
                Email = "bob.nolastname@mail.example.com",
                Phone = "+16135091111",
                FirstName = "John",

                LastOrderId = 234132602919,
                LastOrderName = "#1169",
                MultipassIdentifier = null,
                Note = "Placed an order that had a fraud warning",
                OrdersCount = 3,
                State = "disabled",
                Tags = "loyal",
                TaxExempt = true,
                TotalSpent = 375.30M,
                UpdatedAt = DateTime.Parse("2012-08-24T14:01:46-04:00"),
                VerifiedEmail = true,
                TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },

                Addresses = new()
                {
                    new()
                    {

                        FirstName = "Bob",

                        Address1 = "Chestnut Street 92",
                        Address2 = "Apartment 2",
                        City = "Louisville",
                        Province = "Kentucky",
                        Country = "United States",
                        Zip = "40202",
                        Phone = "555-625-1199",
                        ProvinceCode = "KY",
                        CountryCode = "US",
                        CountryName = "United States",
                        Default = true
                    }

                }



            };

            // Act
            var result = _domain.Add(customer);

            // Assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
            Assert.True(result.Id > 0);
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


        // Add Customer with multiple concurrency (10 times) - create a logic to add 10 data of customers at the same time
        [Fact]
        public void AddTenCustomersAtTheSameTime_Success()
        {

            var numberGenerator = new RandomNumberGenerator();
            // Create an array of 10 customer objects with unique data
            var customers = Enumerable.Range(0, 10).Select(i => new KTI.Moo.Extensions.Shopify.Model.Customer
            {
                Currency = "JPY",
                CreatedAt = DateTime.Now,
                Email = $"customers{numberGenerator.GetNumber()}@example.com",
                Phone = $"+123456412{numberGenerator.GetNumber()}",
                FirstName = $"Customer{i}",
                LastName = "LastName",
                LastOrderId = i,
                LastOrderName = $"#Order{i}",
                MultipassIdentifier = null,
                Note = $"Customer{i} note",
                OrdersCount = 3,
                State = "disabled",
                Tags = "loyal",
                TaxExempt = true,
                TotalSpent = 375.30M,
                UpdatedAt = DateTime.Now,
                VerifiedEmail = true,
                TaxExemptions = new string[] { "CA_STATUS_CARD_EXEMPTION", "CA_BC_RESELLER_EXEMPTION" },
            }).ToArray();

            // Create an array of tasks to add customers concurrently
            var tasks = customers.Select(customer => Task<KTI.Moo.Extensions.Shopify.Model.Customer>.Factory.StartNew(() => _domain.Add(customer))).ToArray();

            // Wait for all tasks to complete asynchronously
            Task.WaitAll(tasks);

            // Retrieve the results of the tasks
            var results = tasks.Select(task => task.Result).ToList();

            // Assert
            foreach (var result in results)
            {
                Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
                Assert.True(result.Id > 0);
            }



        }


        [Fact]
        public void UpsertCustomer_Success()
        {
            //assemble
            var customer = new KTI.Moo.Extensions.Shopify.Model.Customer()
            {
                Id = 7300699980095,
                Currency = "PHP",
                Email = "james2.test@test.com",
                Phone = "+639292948877",
                FirstName = "James",
                LastName = "Test",



            };

            //act
            var result = _domain.Upsert(customer);

            //assert
            Assert.IsAssignableFrom<KTI.Moo.Extensions.Shopify.Model.Customer>(result);
        }






    }
}
