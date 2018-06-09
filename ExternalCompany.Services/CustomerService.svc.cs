using ExternalCompany.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ExternalCompany.Services
{
    public class CustomerService : ICustomerService
    {
        public Customer GetCustomer(int customerId)
        {
            var customers = LoadCustomers();
            return customers.SingleOrDefault(cust => cust.CustomerId == customerId);
        }

        private IEnumerable<Customer> LoadCustomers()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "customers.json");
            var customers = JsonConvert.DeserializeObject<Customer[]>(File.ReadAllText(filePath));
            return customers;
        }
    }
}
