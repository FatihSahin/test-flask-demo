using ExternalCompany.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ExternalCompany.Services
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        Customer GetCustomer(int customerId);
    }
}
