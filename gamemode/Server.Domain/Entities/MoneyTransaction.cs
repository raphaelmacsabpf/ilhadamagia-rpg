using Server.Domain.Enums;
using System;

namespace Server.Domain.Entities
{
    public class MoneyTransaction
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public long Ammount { get; set; }
        public MoneyTransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public MoneyTransactionStatus Status { get; set; }

        public MoneyTransaction(int senderId, int receiverId, long ammount, MoneyTransactionType type, DateTime createdAt, DateTime? finishedAt)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Ammount = ammount;
            Type = type;
            CreatedAt = createdAt;
            FinishedAt = finishedAt;
            this.Status = MoneyTransactionStatus.INIT;
        }
    }
}