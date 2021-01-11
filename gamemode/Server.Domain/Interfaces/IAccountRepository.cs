using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<int> Create(Account account);

        Task<IEnumerable<Account>> GetAccountList(string license);

        IEnumerable<Account> GetAll();

        Task Update(Account account);
    }
}