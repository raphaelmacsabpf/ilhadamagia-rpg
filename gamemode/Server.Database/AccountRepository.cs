using Dapper;
using MySqlConnector;
using Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Database
{
    public class AccountRepository
    {
        private MySqlConnection mySqlConnection;

        public AccountRepository(MySqlConnectionPool mySqlConnectionPool)
        {
            this.mySqlConnection = mySqlConnectionPool.GetRandomConnection();
        }

        public Task<IEnumerable<Account>> GetAccountListByLicense(string license)
        {
            var sqlStatement = @"
SELECT * FROM imtb_account
WHERE License=@License;";
            var accountsTask = this.mySqlConnection.QueryAsync<Account>(sqlStatement, new { License = license });
            return accountsTask;
        }

        public Task<int> Create(Account account)
        {
            var sqlStatement = @"
INSERT INTO imtb_account
(License, Username, Password, CreatedAt)
VALUES (@License, @Username, @Password, @CreatedAt);
SELECT LAST_INSERT_ID();";

            var insertedIdTask = this.mySqlConnection.ExecuteScalarAsync<int>(sqlStatement, account);
            return insertedIdTask;
        }

        public Task Update(Account account)
        {
            account.UpdatedAt = DateTime.Now;
            var sqlStatement = @"
UPDATE imtb_account SET
UpdatedAt = @UpdatedAt,
AdminLevel = @AdminLevel,
DonateRank = @DonateRank,
Level = @Level,
Respect = @Respect,
ConnectedTime = @ConnectedTime,
Money = @Money,
Bank = @Bank
WHERE Id = @Id";

            return this.mySqlConnection.ExecuteAsync(sqlStatement, account);
        }

        public IEnumerable<Account> GetAll()
        {
            return this.mySqlConnection.Query<Account>("SELECT * FROM imtb_account;");
        }
    }
}