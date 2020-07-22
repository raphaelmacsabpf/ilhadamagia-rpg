using System;

namespace Server.Domain.Entities
{
    public class Account
    {
        public Account()
        {
        }

        public int Id { get; set; }
        public string License { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}