using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExternalCompany.Services.Models
{
    public class Account
    {
        public int CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public bool IsOpen { get; set; }
        public decimal Balance { get; set; }
    }
}