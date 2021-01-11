using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Threading.Tasks;

namespace Server.Database.Repositories
{
    public class MoneyTransactionRepository : IMoneyTransactionRepository
    {
        private readonly MySqlConnection mySqlConnection;

        public MoneyTransactionRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public Task Create(MoneyTransaction moneyTransaction)
        {
            const string sqlStatement = @"
                INSERT INTO imtb_money_transaction
                (SenderId, ReceiverId, Ammount, Type, CreatedAt, FinishedAt, Status)
                VALUES
                (@SenderId, @ReceiverId, @Ammount, @Type, @CreatedAt, @FinishedAt, @Status);";

            var values = new
            {
                moneyTransaction.SenderId,
                moneyTransaction.ReceiverId,
                moneyTransaction.Ammount,
                Type = moneyTransaction.Type.ToString(),
                moneyTransaction.CreatedAt,
                moneyTransaction.FinishedAt,
                Status = moneyTransaction.Status.ToString()
            };

            return this.mySqlConnection.ExecuteAsync(sqlStatement, values);
        }
    }
}