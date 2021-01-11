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
                this.chatManager.PlayerScream(commandValidator.SourceGFPlayer, messageToScream);
            }
        }
    }
}
