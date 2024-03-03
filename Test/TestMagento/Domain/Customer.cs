using KTI.Moo.Extensions.Magento.Domain;
using KTI.Moo.Extensions.Magento.Exception;
using KTI.Moo.Extensions.Magento.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestMagento.Model;
using Xunit;

namespace TestMagento.Domain
{
    public class Customer : MagentoBase
    {

        private readonly IDistributedCache _cache;

        public Customer()
        {
            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();

        }


        //[Fact]
        //public void GetCustomerWorking()
        //{

        //    var customerID = "658029";
        //    KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig, _cache);
        //    var response = GetCustomer(customerID, MagentoCustomerDomain);

        //    Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Customer>(response);
        //}

        [Fact]
        public void AddCustomerWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);

            KTI.Moo.Extensions.Magento.Model.Customer CustomerModel = new();

            CustomerModel.email = "kti12341213@example.com";
            CustomerModel.firstname = "Sana";
            CustomerModel.lastname = "OL";
            //   CustomerModel.password = "Password123";

            // List<KTI.Moo.Extensions.Magento.Model.EmailAddress> EmailAddressList = new() { new KTI.Moo.Extensions.Magento.Model.EmailAddress() { primary = true, emailaddress = "bryan1201112@example.com" } };

            ///crmCustomer.address1_telephone1
            List<KTI.Moo.Extensions.Magento.Model.Telephone> telephoneList = new() { new KTI.Moo.Extensions.Magento.Model.Telephone() { primary = true, telephone = "212-555-1111" } };

            //   CustomerModel.EmailAddress = EmailAddressList;

            CustomerModel.address = new List<KTI.Moo.Extensions.Magento.Model.Address>() {
                                      new KTI.Moo.Extensions.Magento.Model.Address(){
                                                    
                                                    //crmCustomer.firstname
                                                    first_name = "Sana",
                                                    //crmCustomer.lastname
                                                    last_name = "OL",
                                                    //crmCustomer.firstname
                                                    defaultShipping = true,
                                                    defaultBilling = true,
                                                    //crmCustomer.firstname
                                                    Telephone = telephoneList,
                                                    //crmCustomer.address1_line1, crmCustomer.address1_line2, crmCustomer.address1_line3 
                                                    street = HelperToArray("123 okay st"),
                                                    //crmCustomer.address1_city
                                                    address_city = "Purchase",
                                                    //GetCountryID(crmCustomer.address_country)
                                                    country_id = "PH",
                                                    //crmCustomer.address1_postalcode
                                                    address_postalcode = "1923",
                                                    region = new KTI.Moo.Extensions.Magento.Model.Region() {
                                                                    //GetRegionID(crmCustomer.address1_stateorprovince)
                                                                    region_id = "886",
                                                                    region_name = "RIZAL",
                                                                    region_code = "REGION IV-A (CALABARZON)"
                                                            },
                                                    custom_attributes = new()
                                                    {
                                                        new()
                                                        {

                                                            attribute_code = "email",
                                                            value =  CustomerModel.email
                                                        }

                                                    }

                                                    }
                                      };


            var response = MagentoCustomerDomain.Add(CustomerModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Customer>(response);

            Assert.NotNull(response.customer_id.ToString());
        }


        [Fact]
        public void UpdateCustomerThrowExcemptions()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);

            KTI.Moo.Extensions.Magento.Model.Customer CustomerModel = new();

            CustomerModel.customer_id = 27;
            //   CustomerModel.email = "bryan1011@example.com";
            CustomerModel.firstname = "SanaTalaga";

            List<KTI.Moo.Extensions.Magento.Model.EmailAddress> EmailAddressList = new()
            {
                new KTI.Moo.Extensions.Magento.Model.EmailAddress()
                {
                    primary = true,
                    emailaddress = "bryan1011@example.com"
                }
            };
            CustomerModel.EmailAddress = EmailAddressList;

            CustomerModel.address = new List<KTI.Moo.Extensions.Magento.Model.Address>() {
                                      new KTI.Moo.Extensions.Magento.Model.Address(){
                                                      address_id=14,
                                                    first_name = "Sana",


                                                    }
                                      };

            Action act = () => MagentoCustomerDomain.Update(CustomerModel);



            MagentoIntegrationException exception = Assert.Throws<MagentoIntegrationException>(act);
        }



        [Fact]
        public void UpdateCustomerWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);

            KTI.Moo.Extensions.Magento.Model.Customer CustomerModel = new();

            CustomerModel.customer_id = 27;
            CustomerModel.firstname = "Sana";


            CustomerModel.address = new List<KTI.Moo.Extensions.Magento.Model.Address>() {
                                      new KTI.Moo.Extensions.Magento.Model.Address(){
                                                    address_id=14,
                                                    first_name = "Sana",
                                                    address_line1 = "Ockratic street"
                                                    }
                                      };

            var response = MagentoCustomerDomain.Update(CustomerModel);


            Assert.True(response);
        }





        [Fact]
        public void DeleteCustomerWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);

            var customerid = 25;

            var response = MagentoCustomerDomain.Delete(customerid);


            Assert.True(response);
        }



        [Fact]
        public void DeleteCustomerThrowExcemption()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);

            var customerid = 25;

            Action act = () => MagentoCustomerDomain.Delete(customerid);

            MagentoIntegrationException exception = Assert.Throws<MagentoIntegrationException>(act);
        }


        private string[] HelperToArray(params string[] arguments)
        {
            return arguments;
        }




        //[Fact]
        //public void CheckGetIDs()
        //{
        //    var CustomerID1 = 1;
        //    var CustomerID2 = 2;

        //    var username = "nova-admin";
        //    var password = "passw0rd";

        //    var redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False";

        //    KTI.Moo.Extensions.Magento.Domain.MagentoCustomer<KTI.Moo.Extensions.Magento.Model.Customer> MagentoCustomerDomain = new(defaultURL, redisConnectionString, username, password);


        //    Type type = typeof(MagentoCustomer<KTI.Moo.Extensions.Magento.Model.Customer>);
        //    var MagentoCustomer = Activator.CreateInstance(type);

        //    //act
        //    MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |BindingFlags.Static)
        //    .Where(x => x.Name == "GetCustomerIDs" && x.IsPrivate && x.IsStatic)
        //    .First();



        //   var Response = (string)method.Invoke(MagentoCustomer, new object[] { CustomerID1, CustomerID2});

        //    Assert.Equal("1, 2", Response);
        //}


        [Fact]
        public void CustomerDateRangeWorking()
        {

            DateTime datfrom = new DateTime(2022, 2, 10);
            DateTime dateto = new DateTime(2022, 3, 14);


            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);


            var response = MagentoCustomerDomain.GetCustomers(datfrom, dateto, 10, 2);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Customer>>(response);
        }


        [Fact]
        public void CustomerDateRangeWorkingGetALL()
        {

            DateTime datfrom = new DateTime(2023, 1, 1);
            DateTime dateto = new DateTime(2023, 1, 30);


            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);

            var CustomerList = new List<KTI.Moo.Extensions.Magento.Model.Customer>();

            var response = MagentoCustomerDomain.GetAll(CustomerList, datfrom, dateto, 1, 1);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Customer>>(response);
        }

        [Fact]
        public void CustomerDateRangeWorkingGetALL_Default()
        {

            DateTime datfrom = new DateTime(2023, 1, 1);
            DateTime dateto = new DateTime(2023, 1, 30);


            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);

            var response = MagentoCustomerDomain.GetAll(datfrom, dateto, 1);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.Magento.Model.Customer>>(response);
        }


        [Fact]
        public void SearchCustomerDateRangeWorking()
        {

            DateTime datfrom = new DateTime(2022, 07, 29);
            DateTime dateto = new DateTime(2022, 07, 30);


            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);


            var response = MagentoCustomerDomain.GetSearchCustomers(datfrom, dateto, 1000, 1);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.DTO.Customers.Search>(response);
        }

        [Fact]
        public void SearchCustomerEmailWorking()
        {

            string email = "brayan033th@gmail.com";

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(config);


            var response = MagentoCustomerDomain.GetSearchCustomersWithEmail(email);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.DTO.Customers.Search>(response);
        }




        [Fact]
        public void UpsertCustomerWorking()
        {
            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);
            KTI.Moo.Extensions.Magento.Model.Customer CustomerModel = new();
            CustomerModel.email = "den.cruz@kationtechnologies.com";
            CustomerModel.firstname = "c";
            CustomerModel.lastname = "Cruz";

            //   CustomerModel.password = "Password123";

            // List<KTI.Moo.Extensions.Magento.Model.EmailAddress> EmailAddressList = new() { new KTI.Moo.Extensions.Magento.Model.EmailAddress() { primary = true, emailaddress = "bryan1201112@example.com" } };

            ///crmCustomer.address1_telephone1
            List<KTI.Moo.Extensions.Magento.Model.Telephone> telephoneList = new() { new KTI.Moo.Extensions.Magento.Model.Telephone() { primary = true, telephone = "212-555-1111" } };

            //   CustomerModel.EmailAddress = EmailAddressList;

            CustomerModel.address = new List<KTI.Moo.Extensions.Magento.Model.Address>() {
                                      new KTI.Moo.Extensions.Magento.Model.Address(){
                                                    
                                                    //crmCustomer.firstname
                                                    first_name = "Den",
                                                    //crmCustomer.lastname
                                                    last_name = "Cruz",
                                                    //crmCustomer.firstname
                                                    defaultShipping = true,
                                                    defaultBilling = true,
                                                    //crmCustomer.firstname
                                                    Telephone = telephoneList,
                                                    //crmCustomer.address1_line1, crmCustomer.address1_line2, crmCustomer.address1_line3 
                                                    address_line1= "123 okay st",
                                                    //street = HelperToArray("123 okay st"),
                                                    //crmCustomer.address1_city
                                                    address_city = "Purchase",
                                                    //GetCountryID(crmCustomer.address_country)
                                                    country_id = "PH",
                                                    //crmCustomer.address1_postalcode
                                                    address_postalcode = "1923",
                                                    //region = new KTI.Moo.Extensions.Magento.Model.Region() {
                                                    //                //GetRegionID(crmCustomer.address1_stateorprovince)
                                                    //                region_id = "886",
                                                    //                region_name = "RIZAL",
                                                    //                region_code = "REGION IV-A (CALABARZON)"
                                                    //},
                                                    custom_attributes = new()
                                                    {
                                                        new()
                                                        {

                                                            attribute_code = "email",
                                                            value =  CustomerModel.email
                                                        }

                                                    }

                                                    }
                                      };


            var response = MagentoCustomerDomain.Upsert(CustomerModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.Magento.Model.Customer>(response);

            Assert.NotNull(response.customer_id.ToString());
        }


        [Fact]
        public void UpdateByGetCustomerWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig, _cache);

            var CustomerModel = MagentoCustomerDomain.Get(658029);

            CustomerModel.firstname = "Danielle";

            var response = MagentoCustomerDomain.Update(CustomerModel);


            Assert.True(response);
        }

        [Fact]
        public void UpdateByCMIDONLYCustomerWorking()
        {

            KTI.Moo.Extensions.Magento.Domain.Customer MagentoCustomerDomain = new(Stagingconfig);


            var CustomerModels = MagentoCustomerDomain.Get(658029);


            KTI.Moo.Extensions.Magento.Model.Customer CustomerModel = new();

            CustomerModel.customer_id = 658029;
            CustomerModel.taxvat = "N0001095";

            var response = MagentoCustomerDomain.Update(CustomerModel);


            Assert.True(response);
        }










    }
}
