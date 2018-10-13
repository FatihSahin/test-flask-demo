using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects.Identifiers;

namespace Demo.QuickPay.Biz.TestFlask
{
    public class AccountRequestIdentifier : IRequestIdentifier<string>
    {
        public string ResolveDisplayInfo(string accountNumber)
        {
            return $"Account ID => {accountNumber}";
        }

        public string ResolveIdentifierKey(string accountNumber)
        {
            return accountNumber; 
        }
    }
}
