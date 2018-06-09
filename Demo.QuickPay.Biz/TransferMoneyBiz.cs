using Demo.QuickPay.Biz.ExternalAccountServices;
using Demo.QuickPay.Biz.ExternalCustomerServices;
using Demo.QuickPay.Biz.Integrators;
using Demo.QuickPay.Biz.Models;
using Demo.QuickPay.Data.Context;
using Demo.QuickPay.Data.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.QuickPay.Biz
{
    public class TransferMoneyBiz
    {
        private readonly FeeCalculator feeCalculator;
        private readonly AccountIntegrator accountIntegrator;
        private readonly CustomerIntegrator customerIntegator;

        public TransferMoneyBiz()
        {
            feeCalculator = new FeeCalculator();
            accountIntegrator = new AccountIntegrator();
            customerIntegator = new CustomerIntegrator();
        }

        public PaymentResult TransferMoney(Payment payment)
        {
            //Check customer
            Customer customer = customerIntegator.GetCustomer(payment.CustomerId);

            if (customer == null)
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "CUSTOMER_NOT_FOUND", ErrorMessage = "Customer was not found." };
            }

            //Check credit account
            Account creditAccount = accountIntegrator.GetAccount(payment.CreditAccount);

            if (creditAccount == null)
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "CREDIT_ACC_NOT_FOUND", ErrorMessage = "Credit account was not found." };
            }

            if (!creditAccount.IsOpen)
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "CREDIT_ACC_CLOSED", ErrorMessage = "Credit account is closed." };
            }

            //Check debit account status
            Account debitAccount = accountIntegrator.GetAccount(payment.DebitAccount);

            if (debitAccount == null)
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "DEBIT_ACC_NOT_FOUND", ErrorMessage = "Credit account was not found." };
            }

            if (!debitAccount.IsOpen)
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "DEBIT_ACC_CLOSED", ErrorMessage = "Debit account was not found." };
            }

            //Calculate fee
            Fee fee = feeCalculator.CalculateFee(customer);

            //Check balance
            if (debitAccount.Balance < (payment.Amount + fee.FeeAmount))
            {
                return new PaymentResult { IsSuccessful = false, ErrorCode = "BALANCE_NOT_AVAILABLE", ErrorMessage = "Debit account balance is not available." };
            }

            //Persist payment
            PaymentRepo paymentRepo = new PaymentRepo();
            paymentRepo.Insert(payment);

            return new PaymentResult { IsSuccessful = true };
        }
    }
}
