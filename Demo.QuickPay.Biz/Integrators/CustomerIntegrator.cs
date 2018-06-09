using Demo.QuickPay.Biz.ExternalCustomerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.QuickPay.Biz.Integrators
{
    public class CustomerIntegrator
    {
        public Customer GetCustomer(int customerId)
        {
            using (var customerClient = new CustomerServiceClient())
            {
                return customerClient.GetCustomer(customerId);
            }
        }
    }
}
