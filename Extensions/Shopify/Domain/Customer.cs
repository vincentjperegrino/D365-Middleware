using KTI.Moo.Extensions.Shopify.Service;
using Microsoft.Extensions.Azure;
using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace KTI.Moo.Extensions.Shopify.Domain;

public class Customer : KTI.Moo.Extensions.Core.Domain.ICustomer<Model.Customer>
{


    private readonly CustomerService _service;



    public Customer(Config config)
    {
        _service = new(config.defaultURL, config.admintoken);

    }

    public Model.Customer Add(Model.Customer customerDetails)
    {
        try
        {
            var Customer = new Model.DTO.Customer(customerDetails);
            var result = _service.CreateAsync(Customer).GetAwaiter().GetResult();
            var CustomerModel = new Model.Customer(result);
            return CustomerModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Add. {ex.Message}");
        }
    }

    //public Model.Customer Add(Model.Customer customerDetails)
    //{
    //    try
    //    {
    //        var Customer = new Model.DTO.Customer(customerDetails);
    //        var result = _service.CreateAsync(Customer).GetAwaiter().GetResult();
    //        var CustomerModel = new Model.Customer(result);
    //        return CustomerModel;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"Extension Shopify Add. {ex.Message}");
    //    }
    //}



    public bool Delete(Model.Customer customer)
    {
        try
        {
            _service.DeleteAsync(customer.Id).GetAwaiter().GetResult();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Delete. {ex.Message}");
        }
    }


    public bool Delete(int customerID)
    {
        try
        {
            _service.DeleteAsync(customerID).GetAwaiter().GetResult();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Delete. {ex.Message}");
        }
    }



    public Model.Customer Get(string customerID)
    {
        try
        {
            var DTOCustomerList = _service.ListAsync().GetAwaiter().GetResult();

            if (DTOCustomerList is null)
            {
                return new Model.Customer();
            }

            var CustomerSearchedByID = DTOCustomerList.Items.FirstOrDefault(customer => customer.Id.ToString() == customerID);

            if (CustomerSearchedByID is null)
            {
                return new Model.Customer();
            }

            return new Model.Customer(CustomerSearchedByID);
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by customerId. {ex.Message}");
        }
    }


    public Model.Customer Get(int customerID)
    {
        try
        {
            var CustomerDTO = _service.GetAsync(customerID).GetAwaiter().GetResult();

            if (CustomerDTO is null)
            {
                return new Model.Customer();
            }

            var Customer = new Model.Customer(CustomerDTO);

            return Customer;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by customerId. {ex.Message}");
        }
    }

    public Model.Customer Get(long customerID)
    {
        try
        {
            var CustomerDTO = _service.GetAsync(customerID).GetAwaiter().GetResult();

            if (CustomerDTO is null)
            {
                return new Model.Customer();
            }

            var Customer = new Model.Customer(CustomerDTO);

            return Customer;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by customerId. {ex.Message}");
        }
    }


    public Model.Customer GetByEmail(string customerEmail)
    {
        try
        {
            var customerFilter = new Model.DTO.CustomerCustomFilter
            {
                Email = customerEmail
            };

            var customerList = _service.ListAsync(customerFilter).GetAwaiter().GetResult();

            if (customerList?.Items != null && customerList.Items.Any())
            {
                var customerSearchedByEmail = customerList.Items.FirstOrDefault(customer => customer.Email == customerEmail);

                if (customerSearchedByEmail != null)
                {
                    return new Model.Customer(customerSearchedByEmail);
                }
            }

            return new Model.Customer();
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by customer email. {ex.Message}");
        }
    }




    public bool Update(Model.Customer customerDetails)
    {
        try
        {
            var Customer = new Model.DTO.Customer(customerDetails);
            var result = _service.UpdateAsync(customerDetails.Id, Customer).GetAwaiter().GetResult();

            return result.Id == customerDetails.Id;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Update. {ex.Message}");
        }
    }

    public Model.Customer Upsert(Model.Customer customerDetails)
    {
        try
        {
            var CustomerDTO = new Model.DTO.Customer(customerDetails);

            if (customerDetails.Id > 0)
            {
                var ExistingCustomer = Get(customerDetails.Id);

                if (ExistingCustomer.Id == customerDetails.Id)
                {
                    var result = _service.UpdateAsync(customerDetails.Id, CustomerDTO).GetAwaiter().GetResult();
                    var ModelCustomer = new Model.Customer(CustomerDTO);
                    return ModelCustomer;
                }

            }

            var CreateCustomer = _service.CreateAsync(CustomerDTO).GetAwaiter().GetResult();

            var CraeteModelCustomer = new Model.Customer(CreateCustomer);

            return CraeteModelCustomer;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Upsert. {ex.Message}");
        }
    }
 
    //public List<Model.Customer> GetList(string CustomerList)

    //{
    //    try
    //    {
    //        var customersListResult = _service.ListAsync().GetAwaiter().GetResult();
    //        var customers = customersListResult.Items; // Assuming there's a property named Items

    //        var modelCustomers = new List<Model.Customer>();

    //        foreach (var customer in customers)
    //        {
    //            var modelCustomer = new Model.Customer(customer);
    //            modelCustomers.Add(modelCustomer);
    //        }

    //        return modelCustomers;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"Extension Shopify GetList. {ex.Message}");
    //    }
    //}


}



