using Server.Application.Entities;
using Server.Application.Enums;
using Server.Application.Managers;
using Server.Domain.Entities;
using Server.Domain.Interfaces;
using Shared.CrossCutting;
using System.Collections.Generic;

namespace Server.Application.Services
{
    public class OrgService
    {
        private readonly IOrgRepository orgRepository;
        private readonly ChatManager chatManager;

        public OrgService(IOrgRepository orgRepository, ChatManager chatManager)
        {
            this.orgRepository = orgRepository;
            this.chatManager = chatManager;
        }

        public IEnumerable<Org> GetAllOrgs()
        {
            return orgRepository.GetAll();
        }

        public Org GetAccountOrg(Account account)
        {
            return orgRepository.GetOrgFromUsername(account.Username);
        }

        public void InvitePlayerToOrg(GFOrg gfOrg, GFPlayer admin, GFPlayer targetPlayer)
        {
            targetPlayer.FSM.Fire(PlayerConnectionTrigger.SET_TO_SPAWN);
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $" Sua organização foi alterada para {gfOrg.Entity.Name} pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você alterou a organização de {targetPlayer.Account.Username} para {gfOrg.Entity.Name}");
        }

        internal void SetOrgLeader(GFOrg gfOrg, GFPlayer admin, GFPlayer targetPlayer)
        {
            gfOrg.Entity.SetLeader(targetPlayer.Account);
            targetPlayer.FSM.Fire(PlayerConnectionTrigger.SET_TO_SPAWN);
            this.chatManager.SendClientMessage(targetPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você foi setado como lider da organização {gfOrg.Entity.Name} pelo admin {admin.Account.Username}");
            this.chatManager.SendClientMessage(admin, ChatColor.COLOR_LIGHTBLUE, $" Você setou {targetPlayer.Account.Username} como líder da organização {gfOrg.Entity.Name}");
        }
    }
}