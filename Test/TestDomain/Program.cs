using Microsoft.Extensions.Logging;
using System;
using System.Configuration;

namespace TestDomain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Product Price List
            //Domain.Modules.ProductPriceLevel test = new Domain.Modules.ProductPriceLevel(3381);

            //test.add(new Domain.Models.Items.ProductPriceLevel
            //{
            //    amount = 350,
            //    pricelevelid = "Test SAP",
            //    productid = "TESTSAPPROD01",
            //    uomid = "Test"
            //}).GetAwaiter().GetResult();
            #endregion

            #region Product
            Domain.Modules.Product test = new Domain.Modules.Product(3388);

            test.replicate(new Domain.Models.Items.Products
            {
                currentcost = 100,
                defaultuomid = "Test",
                defaultuomscheduleid = "TestSAP",
                description = "Sample test product from SAP",
                isstockitem = false,
                defaultvendor = "Sap",
                taxable = false,
                name = "Test SAP Product",
                price = 100,
                productnumber = "TESTSAPPROD01",
                quantityonhand = 100,
                size = "10, cm, 20, cm, 30, cm, 40, m3",
                suppliername = "Sap",
                validfromdate = DateTime.Today.ToString("yyyy-MM-dd"),
                validtodate = DateTime.MaxValue.ToString("yyyy-MM-dd"),
                vendorid = "VENDOR01",
                vendorname = "Vendor Name",
                vendorpartnumber = "VENDORCATALOG01"
            }).GetAwaiter().GetResult();

            //Domain.Modules.ProductBundle test = new Domain.Modules.ProductBundle(3388);

            //var res = test.add(new Domain.Models.Items.ProductBundle
            //{
            //    currentcost = 100,
            //    defaultuomid = "PC",
            //    defaultuomscheduleid = "PC",
            //    description = "Sample test product from SAP",
            //    isstockitem = false,
            //    defaultvendor = "Sap",
            //    taxable = false,
            //    name = "Test SAP Product",
            //    price = 100,
            //    productnumber = "SER00151",
            //    quantityonhand = 100,
            //    size = "10, cm, 20, cm, 30, cm, 40, m3",
            //    suppliername = "Sap",
            //    validfromdate = DateTime.Today.ToString("yyyy-MM-dd"),
            //    validtodate = DateTime.MaxValue.ToString("yyyy-MM-dd"),
            //    vendorid = "VENDOR01",
            //    vendorname = "Vendor Name",
            //    vendorpartnumber = "VENDORCATALOG01",
            //    parentproductid = "Coffee",
            //    pricelevelid = "SRP",
            //    productstructure = 3
            //}).GetAwaiter().GetResult();

            //Domain.Modules.ProductAssociation _test = new Domain.Modules.ProductAssociation(3388);

            //_test.add(new Domain.Models.Items.ProductAssociation
            //{
            //    productid = "SER00151",
            //    associatedproduct = "ACC0066",
            //    quantity = 2,
            //    productisrequired = 1,
            //    uomid = "PC"
            //}).GetAwaiter().GetResult();
            #endregion

            #region Customer
            //Domain.Modules.Customer test = new Domain.Modules.Customer(3381);

            //test.add(new Domain.Models.Customer.Customer { 
            //    address1_city = "Manila",
            //    address1_country = "Philippines",
            //    address1_county = "Metro Manila",
            //    address1_fax = "555-123-4567",
            //    address1_line1 = "Address Line1",
            //    address1_line2 = "Address Line2",
            //    address1_line3 = "Address Line3",
            //    address1_name = "Address 1",
            //    address1_postalcode = "1008",
            //    address1_postofficebox = "Post office",
            //    address1_primarycontactname = "Primary contact name",
            //    address1_stateorprovince = "NCR",
            //    address1_telephone1 = "7311181",
            //    address1_telephone2 = "7411219",
            //    address2_city = "Manila",
            //    address2_country = "Philippines",
            //    address2_county = "Metro Manila",
            //    address2_fax = "555-123-4567",
            //    address2_line1 = "Address Line1",
            //    address2_line2 = "Address Line2",
            //    address2_line3 = "Address Line3",
            //    address2_name = "Address 1",
            //    address2_postalcode = "1008",
            //    address2_postofficebox = "Post office",
            //    address2_primarycontactname = "Primary contact name",
            //    address2_stateorprovince = "NCR",
            //    address2_telephone1 = "7311181",
            //    address2_telephone2 = "7411219",
            //    address3_city = "Manila",
            //    address3_country = "Philippines",
            //    address3_county = "Metro Manila",
            //    address3_fax = "555-123-4567",
            //    address3_line1 = "Address Line1",
            //    address3_line2 = "Address Line2",
            //    address3_line3 = "Address Line3",
            //    address3_name = "Address 1",
            //    address3_postalcode = "1008",
            //    address3_postofficebox = "Post office",
            //    address3_primarycontactname = "Primary contact name",
            //    address3_stateorprovince = "NCR",
            //    address3_telephone1 = "7311181",
            //    address3_telephone2 = "7411219",
            //    birthdate =new DateTime(1996,12,22).ToString("yyyy-MM-dd"),
            //    company = "Company Name",
            //    creditlimit = 100,
            //    emailaddress1 = "testsap1@gmail.com",
            //    emailaddress2 = "testsap2@gmail.com",
            //    emailaddress3 = "testsap3@gmail.com",
            //    fax = "555-123-4567",
            //    firstname = "Test",
            //    gendercode = 1,
            //    jobtitle = "Test Job Title",
            //    lastname = "SAP",
            //    middlename = "Me",
            //    mobilephone = "09123456789",
            //    paymenttermscode = 1,
            //    telephone1 = "7311181",
            //    telephone2 = "7411219",
            //    websiteurl = "https://testsap.com"
            //}).GetAwaiter().GetResult();

            #endregion
        }
    }
}
