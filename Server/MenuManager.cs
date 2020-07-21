using CitizenFX.Core;
using GF.CrossCutting;

namespace Server
{
    public class MenuManager : BaseScript
    {
        private readonly PlayerInfo playerInfo;

        public MenuManager(PlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
        }

        public void OnPlayerMenuAction([FromSource] Player player, int menuActionInt)
        {
            var menuAction = (MenuAction)menuActionInt;
            var gfPlayer = this.playerInfo.GetGFPlayer(player);

            switch (menuAction)
            {
                case MenuAction.CALL_HOUSE_VEHICLE:
                    // TODO: Implementar lógica de chamar carro da casa
                    break;
            }
        }
    }
}