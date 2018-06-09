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
    public class AccountService : IAccountService
    {
        public Account GetAccount(string accountNumber)
        {
            IEnumerable<Account> accounts = LoadAccounts();
            return accounts.SingleOrDefault(acc => acc.AccountNumber == accountNumber);
        }

        private IEnumerable<Account> LoadAccounts()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "accounts.json");
            var accounts = JsonConvert.DeserializeObject<Account[]>(File.ReadAllText(filePath));
            return accounts;
        }
    }
}
