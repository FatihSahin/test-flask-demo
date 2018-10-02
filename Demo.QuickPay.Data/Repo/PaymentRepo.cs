using Demo.QuickPay.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFlask.Aspects;

namespace Demo.QuickPay.Data.Repo
{
    public class PaymentRepo
    {
        [Playback]
        public int Insert(Payment payment)
        {
            using (var db = new QuickPayDb())
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return payment.Id;
            }
        }
    }
}
