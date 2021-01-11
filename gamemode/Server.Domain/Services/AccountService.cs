using Server.Domain.Entities;
using Server.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void EndSession(Account account, float lastPositionX, float lastPositionY, float lastPositionZ, House lastHouseInside)
        {
            account.EndSession(lastPositionX, lastPositionY, lastPositionZ, lastHouseInside);
            this.accountRepository.Update(account);
        }

        public async Task<IEnumerable<Account>> GetAccountListForLicense(string license)
        {
            return await this.accountRepository.GetAccountList(license);
        }
    }
}