using Server.Application.Entities;
using Server.Database;
using Server.Domain.Entities;
using Server.Domain.Enums;
using System;

namespace Server.Application.Services
{
    public class MoneyService
    {
        private const long MaxCash = 2000000000;
        private readonly MoneyTransactionRepository moneyTransactionRepository;

        public MoneyService(MoneyTransactionRepository moneyTransactionRepository)
        {
            this.moneyTransactionRepository = moneyTransactionRepository;
        }

        public MoneyTransaction Pay(GFPlayer sender, GFPlayer receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(sender.Account.Id, receiver.Account.Id, ammount, MoneyTransactionType.PAY, DateTime.Now, DateTime.Now);
            if (sender.Account.Money < ammount)
            {
                moneyTransaction.Status = MoneyTransactionStatus.SENDER_INSUFFICIENT_FUNDS;
            }
            else if (receiver.Account.Money > MaxCash || receiver.Account.Money + ammount > MaxCash)
            {
                moneyTransaction.Status = MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS;
            }
            else
            {
                sender.Account.Money -= ammount;
                receiver.Account.Money += ammount;
                moneyTransaction.Status = MoneyTransactionStatus.FINISHED;
                moneyTransactionRepository.Create(moneyTransaction).Wait();
            }

            return moneyTransaction;
        }

        public MoneyTransaction AdminGiveMoney(GFPlayer admin, GFPlayer receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(admin.Account.Id, receiver.Account.Id, ammount, MoneyTransactionType.ADMIN_GIVE_MONEY, DateTime.Now, DateTime.Now);
            if (receiver.Account.Money > MaxCash || receiver.Account.Money + ammount > MaxCash)
            {
                moneyTransaction.Status = MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS;
            }
            else
            {
                receiver.Account.Money += ammount;
                moneyTransaction.Status = MoneyTransactionStatus.FINISHED;
                moneyTransactionRepository.Create(moneyTransaction).Wait();
            }

            return moneyTransaction;
        }
    }
}