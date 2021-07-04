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
                PlayerHandle targetPlayerHandle = commandValidator.GetTargetPlayerHandle();
                var money = commandValidator.GetVar<long>("money");

                var distanceSquared = commandValidator.SourcePlayerHandle.Player.Character.Position.DistanceToSquared(targetPlayerHandle.Player.Character.Position);
                var maximumDistanceSquared = Math.Pow(5.0d, 2);
                if (distanceSquared > maximumDistanceSquared)
                {
                    this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_GRAD2, $"Jogador muito longe");
                    return;
                }

                if (commandValidator.SourcePlayerHandle != targetPlayerHandle)
                {
                    var moneyTransaction = moneyService.Pay(commandValidator.SourcePlayerHandle.Account, targetPlayerHandle.Account, money);
                    if (moneyTransaction.Status == MoneyTransactionStatus.SENDER_INSUFFICIENT_FUNDS)
                    {
                        this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_GRAD2, $"* Dinheiro insuficiente. Você possui ${targetPlayerHandle.Account.Money} de dinheiro no momento."); // TODO: Criar formatação para dinheiro
                        return;
                    }

                    if (moneyTransaction.Status == MoneyTransactionStatus.RECEIVER_AT_MAXIMUM_FUNDS)
                    {
                        this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_GRAD2, $"* {targetPlayerHandle.Account.Username} não pode receber esta quantia no momento.");
                        return;
                    }

                    this.playerInfo.SendUpdatedPlayerVars(targetPlayerHandle); // TODO: Resolver essa pendencia do sendupdatedvars, arranjar alternativa melhor
                    this.playerInfo.SendUpdatedPlayerVars(commandValidator.SourcePlayerHandle); // TODO: Resolver essa pendencia do sendupdatedvars, arranjar alternativa melhor
                    var ignoredPlayersInChatEvent = new[] { commandValidator.SourcePlayerHandle, targetPlayerHandle };
                    this.chatManager.ProxDetectorColorFixed(10.0f, commandValidator.SourcePlayerHandle, $" * {commandValidator.SourcePlayerHandle.Account.Username} pagou para {targetPlayerHandle.Account.Username}", ChatColor.COLOR_PURPLE, ignoredPlayersInChatEvent);
                    this.chatManager.SendClientMessage(targetPlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" O jogador {commandValidator.SourcePlayerHandle.Account.Username} lhe pagou ${money} em dinheiro");
                }

                this.chatManager.SendClientMessage(commandValidator.SourcePlayerHandle, ChatColor.COLOR_LIGHTBLUE, $" Você pagou ${money} em dinheiro para {targetPlayerHandle.Account.Username}");
            }
        }
    }
}