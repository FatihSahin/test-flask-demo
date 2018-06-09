using Demo.QuickPay.Biz.ExternalAccountServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.QuickPay.Biz.Integrators
{
    public class AccountIntegrator
    {
        public Account GetAccount(string accountNumber)
        {
            using (var accountClient = new ExternalAccountServices.AccountServiceClient())
            {
                return accountClient.GetAccount(accountNumber);
            }
        }
    }
}
