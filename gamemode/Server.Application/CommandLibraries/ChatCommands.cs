using Server.Application.Managers;

namespace Server.Application.CommandLibraries
{
    public class ChatCommands : CommandLibrary
    {
        private readonly ChatManager chatManager;

        public ChatCommands(ChatManager chatManager)
        {
            this.chatManager = chatManager;
        }

        [Command("/gritar")]
        public void Scream(CommandValidator commandValidator)
        {
            if (commandValidator.WithVarText("scream-text").IsValid("USE: /gritar [mensagem]"))
            {
                var messageToScream = commandValidator.GetVar<string>("scream-text");
                this.chatManager.PlayerScream(commandValidator.SourcePlayerHandle, messageToScream);
            }
        }

        [Command("/dm")] //TODO: Restrict as /par in BPF
        public void DirectMessage(CommandValidator commandValidator)
        {
            if (commandValidator.WithTargetPlayer("target").WithVarText("text").IsValid("USE: /dm [playerid] [mensagem]"))
            {
                var target = commandValidator.GetTargetPlayerHandle();
                var source = commandValidator.SourcePlayerHandle;
                var message = commandValidator.GetVar<string>("text");
                this.chatManager.SendClientMessage(target, Shared.CrossCutting.ChatColor.COLOR_YELLOW, $"{source.Account.Username}(ID: {source.Player.Handle}) DM: {message}");
                this.chatManager.SendClientMessage(source, Shared.CrossCutting.ChatColor.COLOR_YELLOW, $"Sua DM foi enviada para {source.Account.Username}(ID: {source.Player.Handle})");
            }
        }
    }
}