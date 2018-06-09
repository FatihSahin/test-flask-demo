using Demo.QuickPay.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.QuickPay.Data.Repo
{
    public class FeeRepo
    {
        public Fee GetFee(FeeType feeType)
        {
            using (var db = new QuickPayDb())
            {
                return db.Fees.SingleOrDefault(fee => fee.FeeType == feeType);
            }
        }
    }
}
