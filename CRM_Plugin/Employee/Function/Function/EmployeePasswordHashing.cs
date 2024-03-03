
using System;
using Microsoft.Xrm.Sdk;
using CRM_Plugin.Domain;

namespace CRM_Plugin.Function
{
    public class EmployeePasswordHashing
    {
        private readonly ITracingService _tracingService;
        private readonly IEmployee _employee;

        public EmployeePasswordHashing(IEmployee employee, ITracingService tracingService)
        {
            _tracingService = tracingService;
            _employee = employee;
        }


        public bool ProcessAppPassword(Models.Employee employee)
        {
            try
            {
                return HashPassword(employee);

            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password: " + ex.Message);
            }
        }

        private bool HashPassword(Models.Employee employee)
        {
            return _employee.HashPassword(employee);
        }
    }
}


