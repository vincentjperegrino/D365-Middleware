using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Plugin.CustomAPI.Function
{
    public class UpsertCustomer
    {

        public readonly Domain.Customer _customer;



        public UpsertCustomer(IOrganizationService service, ITracingService tracingService)
        {
            this._customer = new Domain.Customer(service, tracingService);
           
        }


        public UpsertCustomer(IOrganizationService service)
        {
            this._customer = new Domain.Customer(service);
        }

        public string Process(ParameterCollection customAPIProduct)
        {
            if (customAPIProduct is null)
            {
                throw new ArgumentNullException(nameof(customAPIProduct));
            }

            string erroron = "";

            try
            {
                //erroron = "Data mapping of custom API parameters. ";

                //var dtoProduct = new CRM_Plugin.CustomAPI.Model.DTO.Product(customAPIProduct);

                erroron = "Upsert customer process to D365";

                if (_customer.Upsert((Entity)customAPIProduct["kti_customerDTO"]))
                {
                    return "Success";
                }

                return "Failed";
            }
            catch (Exception ex)
            {
                throw new Exception(erroron + ex);
            }
        }
    }
}
