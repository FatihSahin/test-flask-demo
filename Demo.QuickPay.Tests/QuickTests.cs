using System;
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

    }
}
