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

        static TransferMoneyBiz()
        {
            InitErrors();
        }

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
                return ProvideErrorResult(CUSTOMER_NOT_FOUND);
            }

            //Check credit account
            Account creditAccount = accountIntegrator.GetAccount(payment.CreditAccount);

            if (creditAccount == null)
            {
                return ProvideErrorResult(CREDIT_ACC_NOT_FOUND);
            }

            if (!creditAccount.IsOpen)
            {
                return ProvideErrorResult(CREDIT_ACC_CLOSED);
            }

            //Check debit account status
            Account debitAccount = accountIntegrator.GetAccount(payment.DebitAccount);

            if (debitAccount == null)
            {
                return ProvideErrorResult(DEBIT_ACC_NOT_FOUND);
            }

            if (debitAccount.CustomerId != customer.CustomerId)
            {
                return ProvideErrorResult(DEBIT_ACC_NOT_OWNED);
            }

            if (!debitAccount.IsOpen)
            {
                return ProvideErrorResult(DEBIT_ACC_CLOSED);
            }

            //Calculate fee
            Fee fee = feeCalculator.CalculateFee(customer);

            //Check balance
            if (debitAccount.Balance < (payment.Amount + fee.FeeAmount))
            {
                return ProvideErrorResult(BALANCE_NOT_AVAILABLE);
            }

            //Persist payment
            payment.PaymentDate = DateTime.UtcNow;
            PaymentRepo paymentRepo = new PaymentRepo();
            paymentRepo.Insert(payment);

            return new PaymentResult { IsSuccessful = true };
        }

        #region Errors

        private const string CUSTOMER_NOT_FOUND = "CUSTOMER_NOT_FOUND";
        private const string CREDIT_ACC_NOT_FOUND = "CREDIT_ACC_NOT_FOUND";
        private const string CREDIT_ACC_CLOSED = "CREDIT_ACC_CLOSED";
        private const string DEBIT_ACC_NOT_OWNED = "DEBIT_ACC_NOT_OWNED";
        private const string DEBIT_ACC_NOT_FOUND = "DEBIT_ACC_NOT_FOUND";
        private const string DEBIT_ACC_CLOSED = "DEBIT_ACC_CLOSED";
        private const string BALANCE_NOT_AVAILABLE = "BALANCE_NOT_AVAILABLE";

        private static Dictionary<string, string> errDictionary = new Dictionary<string, string>();
        private static void InitErrors()
        {
            errDictionary.Add(CUSTOMER_NOT_FOUND, "Customer was not found.");
            errDictionary.Add(CREDIT_ACC_NOT_FOUND, "Credit account was not found.");
            errDictionary.Add(CREDIT_ACC_CLOSED, "Credit account is closed."); 
            errDictionary.Add(DEBIT_ACC_NOT_FOUND, "Debit account was not found.");
            errDictionary.Add(DEBIT_ACC_NOT_OWNED, "Debit account is not owned by the customer.");
            errDictionary.Add(DEBIT_ACC_CLOSED, "Debit account is closed.");
            errDictionary.Add(BALANCE_NOT_AVAILABLE, "Debit account balance is not available.");
        }

        private PaymentResult ProvideErrorResult(string errCode)
        {
            return new PaymentResult
            {
                IsSuccessful = false,
                ErrorCode = errCode,
                ErrorMessage = errDictionary[errCode]
            };
        }

        #endregion
    }
}
