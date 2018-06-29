using Demo.QuickPay.Biz;
using Demo.QuickPay.Biz.Models;
using Demo.QuickPay.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestFlask.Aspects;

namespace Demo.QuickPay.WebApi.Controllers
{
    public class PaymentController : ApiController
    {
        [HttpPut]
        [Playback]
        public PaymentResult TransferMoney(Payment payment)
        {
            TransferMoneyBiz transferMoneyBiz = new TransferMoneyBiz();
            return transferMoneyBiz.TransferMoney(payment);
        }
    }
}
