using Demo.QuickPay.Biz.ExternalCustomerServices;
using Demo.QuickPay.Data.Context;
using Demo.QuickPay.Data.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects;

namespace Demo.QuickPay.Biz
{
    public class FeeCalculator
    {
        private readonly FeeRepo feeRepo;
        public FeeCalculator()
        {
            feeRepo = new FeeRepo();
        }

        [Playback]
        public Fee CalculateFee(Customer customer)
        {
            FeeType feeType = customer.IsPremium ? FeeType.PremiumFee : FeeType.RegularFee;
            return feeRepo.GetFee(feeType);
        }
    }
}
