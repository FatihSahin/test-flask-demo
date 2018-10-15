using System;
using Demo.QuickPay.Biz.Models;
using Demo.QuickPay.Data.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestFlask.Models.Entity;

namespace Demo.QuickPay.Tests
{
    public partial class QuickTests
    {
        private static void DoClassSetUp(TestContext context)
        {
        }

        private static void DoClassTearDown()
        {
        }

        private void ProvideOperationContext(Invocation rootInvocation)
        {
        }

        [TestInitialize]
        // will be empty on template 
        public void ProvideSubjects() {

            subjectPaymentController = new WebApi.Controllers.PaymentController();
        }

        private void SetUp_Scenario33_MoneyTransferSuccess_Step49_transferMoneySuccessWithAmount5(Payment payment)
        {
            //no additional setup
        }

        private void Assert_Scenario33_MoneyTransferSuccess_Step49_transferMoneySuccessWithAmount5(PaymentResult paymentResult, Exception exception)
        {
            Assert.IsTrue(paymentResult.IsSuccessful);
        }

        private void SetUp_Scenario34_MoneyTransferFailure_Step50_transferMoneyFailureWithClosedDebitAccount(Payment requestObject)
        {
            //no additional setup
        }

        private void Assert_Scenario34_MoneyTransferFailure_Step50_transferMoneyFailureWithClosedDebitAccount(PaymentResult paymentResult, Exception exception)
        {
            Assert.IsFalse(paymentResult.IsSuccessful);
            Assert.AreEqual("DEBIT_ACC_CLOSED", paymentResult.ErrorCode);
        }
    }
}
