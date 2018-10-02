using Demo.QuickPay.Biz.ExternalAccountServices;
using Demo.QuickPay.Biz.TestFlask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects;

namespace Demo.QuickPay.Biz.Integrators
{
    public class AccountIntegrator
    {
        [Playback(typeof(AccountRequestIdentifier))]
        public Account GetAccount(string accountNumber)
        {
            using (var accountClient = new ExternalAccountServices.AccountServiceClient())
            {
                return accountClient.GetAccount(accountNumber);
            }
        }
    }
}
