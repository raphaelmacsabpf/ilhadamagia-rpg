using Server.Application.Entities;
using Server.Database;
using Server.Domain.Entities;
using System;

namespace Server.Application.Services
{
    public class MoneyService
    {
        private readonly MoneyTransactionRepository moneyTransactionRepository;

        public MoneyService(MoneyTransactionRepository moneyTransactionRepository)
        {
            this.moneyTransactionRepository = moneyTransactionRepository;
        }

        public void Pay(GFPlayer sender, GFPlayer receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(sender.Account.Id, receiver.Account.Id, ammount, "PAY", DateTime.Now, null, "INIT");
            moneyTransactionRepository.Create(moneyTransaction).Wait();
            sender.Account.Money -= ammount;
            receiver.Account.Money += ammount;
        }

        public void AdminGiveMoney(GFPlayer admin, GFPlayer receiver, long ammount)
        {
            var moneyTransaction = new MoneyTransaction(admin.Account.Id, receiver.Account.Id, ammount, "ADMIN_GIVE_MONEY", DateTime.Now, null, "INIT");
            moneyTransactionRepository.Create(moneyTransaction).Wait();
            receiver.Account.Money += ammount;
        }
    }
}