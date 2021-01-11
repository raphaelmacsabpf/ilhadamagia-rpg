using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IHouseRepository
    {
        IEnumerable<House> GetAll();

        Task<IEnumerable<House>> GetAllFromAccount(Account account);
    }
}