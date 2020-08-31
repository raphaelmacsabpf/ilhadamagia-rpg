using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System.Threading.Tasks;

namespace Server.Database
{
    public class MoneyTransactionRepository
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
                (@SenderId, @ReceiverId, @Ammount, @Type, NOW(), NULL, 'INIT');";
            return this.mySqlConnection.ExecuteAsync(sqlStatement, moneyTransaction);
        }
    }
}