using Server.Domain.Entities;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IMoneyTransactionRepository
    {
        Task Create(MoneyTransaction moneyTransaction);
    }
}