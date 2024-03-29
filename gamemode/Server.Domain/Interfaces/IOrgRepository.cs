﻿using Server.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Domain.Interfaces
{
    public interface IOrgRepository
    {
        IEnumerable<Org> GetAll();

        IEnumerable<string> GetOrgMembersById(int orgId);

        Task Update(Org org);
    }
}