using Server.Application.Entities;
using Server.Application.Managers;
using Server.Domain.Enums;
using Server.Domain.Services;
using Shared.CrossCutting;
using System;

namespace Server.Application.CommandLibraries
{
    public class MoneyCommands : CommandLibrary
    {
        private readonly ChatManager chatManager;
        private readonly PlayerInfo playerInfo;
        private readonly MoneyService moneyService;

        public MoneyCommands(ChatManager chatManager, PlayerInfo playerInfo, MoneyService moneyService)
        {
            this.chatManager = chatManager;
            this.playerInfo = playerInfo;
            this.moneyService = moneyService;
        }

        [Command("/pagar")]
        public void Pay(CommandValidator commandValidator)
        {
            if (commandValidator.WithTargetPlayer("receiver")
                                 .WithVarBetween<long>(1, 50000, "money")
                                 .IsValid("USE: /pagar [playerid] [money(1-50000"))
            {
                GFPlayer targetGfPlayer = commandValidator.GetTargetGFPlayer();
                var money = commandValidator.GetVar<long>("money");

                var distanceSquared = commandValidator.SourceGFPlayer.Player.Character.Position.DistanceToSquared(targetGfPlayer.Player.Character.Position);
                var maximumDistanceSquared = Math.Pow(5.0d, 2);
                if (distanceSquared > maximumDistanceSquared)
                {
                    this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD2, $"Jogador muito longe");
                    return;
                }

                if (commandValidator.SourceGFPlayer != targetGfPlayer)
                {
                    var moneyTransaction = moneyService.Pay(commandValidator.SourceGFPlayer.Account, targetGfPlayer.Account, money);
                    if (moneyTransaction.Status == MoneyTransactionStatus.SENDER_INSUFFICIENT_FUNDS)
                    {
                        this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD2, $"* Dinheiro insuficiente. Você possui ${targetGfPlayer.Account.Money} de dinheiro no momento."); // TODO: Criar formatação para dinheiro
                        return;
                    }

                    if (moneyTransaction.Status == MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS)
                    {
                        this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_GRAD2, $"* {targetGfPlayer.Account.Username} não pode receber esta quantia no momento.");
                        return;
                    }

                    this.playerInfo.SendUpdatedPlayerVars(targetGfPlayer); // TODO: Resolver essa pendencia do sendupdatedvars, arranjar alternativa melhor
                    this.playerInfo.SendUpdatedPlayerVars(commandValidator.SourceGFPlayer); // TODO: Resolver essa pendencia do sendupdatedvars, arranjar alternativa melhor
                    var ignoredPlayersInChatEvent = new[] { commandValidator.SourceGFPlayer, targetGfPlayer };
                    this.chatManager.ProxDetectorColorFixed(10.0f, commandValidator.SourceGFPlayer, $" * {commandValidator.SourceGFPlayer.Account.Username} pagou para {targetGfPlayer.Account.Username}", ChatColor.COLOR_PURPLE, ignoredPlayersInChatEvent);
                    this.chatManager.SendClientMessage(targetGfPlayer, ChatColor.COLOR_LIGHTBLUE, $" O jogador {commandValidator.SourceGFPlayer.Account.Username} lhe pagou ${money} em dinheiro");
                }

                this.chatManager.SendClientMessage(commandValidator.SourceGFPlayer, ChatColor.COLOR_LIGHTBLUE, $" Você pagou ${money} em dinheiro para {targetGfPlayer.Account.Username}");
            }
        }
    }
}