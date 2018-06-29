using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects.Identifiers;

namespace Demo.QuickPay.Biz.TestFlask
{
    public class AccountNumberIdentifier : IRequestIdentifier<string>
    {
        public string ResolveDisplayInfo(string accountNumebr)
        {
            return $"Accountnumber:{accountNumebr}"; 
        }

        public string ResolveIdentifierKey(string req)
        {
            return req; 
        }
    }
}
