using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);
            
            if (from==null)
            {
                throw new InvalidOperationException("There is no details for From account details");
            }

            if (to == null)
            {
                throw new InvalidOperationException("There is no details for To account details");
            }

            var fromBalance = from.Balance - amount;
            if (fromBalance <= 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (fromBalance < 500m)
            {
                this.notificationService.NotifyFundslow(from.User.Email);
            }

            var paidIn = to.PaidIn + amount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (Account.PayInLimit - paidIn < 500m)
                {
                    this.notificationService.NotifyApproachingPayInLimit(to.User.Email);
                }

            from.Withdraw(amount);
            to.Deposit(amount);

            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}
