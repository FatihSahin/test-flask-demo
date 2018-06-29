﻿using Demo.QuickPay.Biz.ExternalCustomerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects;

namespace Demo.QuickPay.Biz.Integrators
{
    public class CustomerIntegrator
    {
        [Playback]
        public Customer GetCustomer(int customerId)
        {
            using (var customerClient = new CustomerServiceClient())
            {
                return customerClient.GetCustomer(customerId);
            }
        }
    }
}
