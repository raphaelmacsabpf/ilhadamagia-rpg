using System;

namespace Server.Domain.Entities
{
    public class MoneyTransaction
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public long Ammount { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string Status { get; set; }

        public MoneyTransaction(int senderId, int receiverId, long ammount, string type, DateTime createdAt, DateTime? finishedAt, string status)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Ammount = ammount;
            Type = type;
            CreatedAt = createdAt;
            FinishedAt = finishedAt;
            Status = status;
        }
    }
}