using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExternalCompany.Services.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsPremium { get; set; }
    }
}