using KTI.Moo.Extensions.OctoPOS.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOctoPOS.Model;
using Xunit;


namespace TestOctoPOS.Domain
{
    public class Customer : OctoPOSBase
    {

        private KTI.Moo.Extensions.OctoPOS.Domain.Customer OctoPOSCustomer;
        private readonly IDistributedCache _cache;

        public Customer()
        {

            var services = new ServiceCollection();
            services.AddDistributedRedisCache(o => { o.Configuration = redisConnectionString; });

            var provider = services.BuildServiceProvider();
            _cache = provider.GetService<IDistributedCache>();


        }

        [Fact]
        public void IFworkingGetCustomer()
        {

            var customerCode = "KTI_SAMPLE39";

            var response = OctoPOSCustomer.Get(customerCode);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
        }


        [Fact]
        public void IFworkingAddCustomerCRM_model()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Customer CustomerModel = new();

            CustomerModel.CustomerCode = "477421113939b";
            CustomerModel.firstname = "Maritess";
            CustomerModel.lastname = "yao";
            CustomerModel.Sex = "Female";
            CustomerModel.Location = "WELF01";
            CustomerModel.EmailAddress = new()
            {
                new()
                {
                    primary = true,
                    emailaddress = "mimsyyao@gmail.com"
                }

            };

            CustomerModel.Address = new()
            {
                new()
                {
                    address_name = AddressTypeHelper.billingAddressName,
                    address_line1 = "41 sct Limbaga st. Quezon city",
                    address_line2 = "",
                    address_line3 = "Manila",
                    address_country = "PH",
                    address_postalcode = "1103",
                    Telephone = new()
                    {
                        new()
                        {
                            telephone_type = TelephoneTypeHelpher.HandPhoneType,
                            primary = true,
                            telephone = "09175059109"
                        },
                        new()
                        {
                            telephone_type = TelephoneTypeHelpher.HomePhoneType,
                            primary = true,
                            telephone = "09175059109"

                        },
                        new()
                        {
                            telephone_type = TelephoneTypeHelpher.OfficePhoneType,
                            primary = true,
                            telephone = ""

                        }

                    }

                },
                new()
                {
                    address_name = AddressTypeHelper.shippingAddressName,
                    address_line1 = "41 sct Limbaga st. Quezon city",
                    address_line2 = "",
                    address_line3 = "Manila",
                    address_country = "PH",
                    address_postalcode = "1103",
                    Telephone = new()
                    {
                        new()
                        {
                            telephone_type = TelephoneTypeHelpher.HandPhoneType,
                            primary = true,
                            telephone = "09175059109"
                        }

                    }

                }

            };


            var response = OctoPOSCustomer.Add(CustomerModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
        }

        [Fact]
        public void IFworkingAddCustomer()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Customer CustomerModel = new();

            CustomerModel.CustomerCode = "KTI_SAMPLE123578fas";
            CustomerModel.firstname = "Maharlika";
            CustomerModel.lastname = "Mars";
            CustomerModel.Sex = "Female";
            CustomerModel.Location = "WELF01";
            CustomerModel.Email = "mimsyyao113@gmail.com";
            CustomerModel.Address1 = "41 sct Limbaga st. Quezon city";
            CustomerModel.Address2 = "";
            CustomerModel.Address3 = "Manila";
            CustomerModel.Country = "PH";
            CustomerModel.PostalCode = "1103";
            CustomerModel.HandPhone = "09175059109";
            CustomerModel.HomePhone = "09175059109";
            CustomerModel.OfficePhone = "";
            CustomerModel.firstname = "Maritess";
            CustomerModel.ShippingAddress = "41 sct Limbaga st. Quezon city";
            CustomerModel.ShippingAddress2 = "";
            CustomerModel.ShippingPostalCode = "1103";
            CustomerModel.ShippingCountry = "PH";
            CustomerModel.ShippingContact = "09175059109";
            var response = OctoPOSCustomer.Add(CustomerModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
        }


        [Fact]
        public void IFworkingUpdateCustomer()
        {

            KTI.Moo.Extensions.OctoPOS.Model.Customer CustomerModel = new();

            CustomerModel.CustomerCode = "4774211139391a";
            CustomerModel.firstname = "Maritess";
            CustomerModel.lastname = "yao";
            CustomerModel.Sex = "Female";
            CustomerModel.Location = "WELF01";
            CustomerModel.Email = "mimsyyao@gmail.com";
            CustomerModel.Address1 = "42 sct Limbaga st. Quezon city";
            CustomerModel.Address2 = "";
            CustomerModel.Address3 = "Manila";
            CustomerModel.Country = "PH";
            CustomerModel.PostalCode = "1103";
            CustomerModel.HandPhone = "09175059109";
            CustomerModel.HomePhone = "09175059109";
            CustomerModel.OfficePhone = "";
            CustomerModel.salutation = "Mrs";
            CustomerModel.firstname = "Maritess";
            CustomerModel.ShippingAddress = "41 sct Limbaga st. Quezon city";
            CustomerModel.ShippingAddress2 = "";
            CustomerModel.ShippingPostalCode = "1103";
            CustomerModel.ShippingCountry = "PH";
            CustomerModel.ShippingContact = "09175059109";

            var response = OctoPOSCustomer.Update(CustomerModel);

            Assert.IsAssignableFrom<bool>(response);
        }

        [Fact]
        public void IFworkingUpsertCustomer()
        {

            var customerCode = "KTI_SAMPLE39";

            var CustomerModel = OctoPOSCustomer.Get(customerCode);

            var response = OctoPOSCustomer.Upsert(CustomerModel);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
        }

        [Fact]
        public void IFworkingGetSearchListByDate()
        {
            OctoPOSCustomer = new(ConfigProduction, _cache);
            int PageNumber = 1;
            DateTime startdate = DateTimeHelper.PHTnow();
            DateTime enddate = DateTimeHelper.PHTnow();

            var response = OctoPOSCustomer.GetSearchListByDate(startdate, enddate, PageNumber);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.DTO.Customers.Search>(response);
        }

        [Fact]
        public void GetByEmail_Working()
        {
            OctoPOSCustomer = new(NewProdToStagingconfig, _cache);

            var email = "diannejcdelloma@gmail.com";

            var response = OctoPOSCustomer.GetByEmail(email);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
            Assert.Equal(email, response.Email);
        }


        [Fact]
        public void GetByMobile_Working()
        {
            OctoPOSCustomer = new(NewProdToStagingconfig, _cache);

            var phone = "+639189176500";

            var response = OctoPOSCustomer.GetByPhone(phone);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
            Assert.Equal(phone, response.HandPhone);
        }

        [Fact]
        public void IFworkingGetCustomerProd()
        {
            OctoPOSCustomer = new(ConfigProduction, _cache);
            var customerCode = "663544";

            var response = OctoPOSCustomer.Get(customerCode);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.Customer>(response);
        }


        [Fact]
        public void IFworkingGetCustomerListProd_Search_GetALL()
        {
            OctoPOSCustomer = new(ConfigProduction, _cache);

            var CustomerList = new List<KTI.Moo.Extensions.OctoPOS.Model.Customer>();

            DateTime startdate = DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = DateTimeHelper.PHTnow();

            var response = OctoPOSCustomer.GetAll(CustomerList, startdate, enddate, 100, 1);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Customer>>(response);
        }


        [Fact]
        public void IFworkingGetCustomerListProd_Search_Get()
        {
            OctoPOSCustomer = new(ConfigProduction, _cache);

            DateTime startdate = DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = DateTimeHelper.PHTnow();

            var response = OctoPOSCustomer.Get(startdate, enddate, 100, 1);

            Assert.IsAssignableFrom<KTI.Moo.Extensions.OctoPOS.Model.DTO.Customers.Search>(response);
        }

        [Fact]
        public void IFworkingGetCustomerListProd_Search_GetAllDefault()
        {
            OctoPOSCustomer = new(ConfigProduction, _cache);

            DateTime startdate = DateTimeHelper.PHTnow().AddDays(-1);
            DateTime enddate = DateTimeHelper.PHTnow();

            var response = OctoPOSCustomer.GetAll(startdate, enddate);

            Assert.IsAssignableFrom<List<KTI.Moo.Extensions.OctoPOS.Model.Customer>>(response);
        }
    }
}
