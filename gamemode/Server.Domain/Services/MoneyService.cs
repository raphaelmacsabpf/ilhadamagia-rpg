using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Interfaces;
using System;

namespace Server.Domain.Services
{
    public class MoneyService
    {
        private const long MaxCash = 2000000000;
        private readonly IMoneyTransactionRepository moneyTransactionRepository;

        public MoneyService(IMoneyTransactionRepository moneyTransactionRepository)
        {
            this.moneyTransactionRepository = moneyTransactionRepository;
        }

        public MoneyTransaction Pay(Account sender, Account receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(sender.Id, receiver.Id, ammount, MoneyTransactionType.PAY, DateTime.Now, DateTime.Now);
            if (sender.Money < ammount)
            {
                moneyTransaction.Status = MoneyTransactionStatus.SENDER_INSUFFICIENT_FUNDS;
            }
            else if (receiver.Money > MaxCash || receiver.Money + ammount > MaxCash)
            {
                moneyTransaction.Status = MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS;
            }
            else
            {
                sender.TakeMoney(ammount);
                receiver.GiveMoney(ammount);
                moneyTransaction.Status = MoneyTransactionStatus.FINISHED;
                moneyTransactionRepository.Create(moneyTransaction).Wait();
            }

            return moneyTransaction;
        }

        public MoneyTransaction AdminGiveMoney(Account admin, Account receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(admin.Id, receiver.Id, ammount, MoneyTransactionType.ADMIN_GIVE_MONEY, DateTime.Now, DateTime.Now);
            if (receiver.Money > MaxCash || receiver.Money + ammount > MaxCash)
            {
                moneyTransaction.Status = MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS;
            }
            else
            {
                receiver.GiveMoney(ammount);
                moneyTransaction.Status = MoneyTransactionStatus.FINISHED;
                moneyTransactionRepository.Create(moneyTransaction).Wait();
            }

            return moneyTransaction;
        }
    }
}