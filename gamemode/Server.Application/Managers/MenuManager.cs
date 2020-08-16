using CitizenFX.Core;
using Newtonsoft.Json;
using Shared.CrossCutting;

namespace Server.Application.Managers
{
    public class MenuManager : BaseScript
    {
        private readonly PlayerInfo playerInfo;
        private readonly NetworkManager networkManager;
        private readonly MapManager mapManager;

        public MenuManager(PlayerInfo playerInfo, NetworkManager networkManager, MapManager mapManager)
        {
            this.playerInfo = playerInfo;
            this.networkManager = networkManager;
            this.mapManager = mapManager;
        }

        public void OnPlayerMenuAction([FromSource] Player player, int menuActionInt, string compressedPayload)
        {
            var uncompressedPayload = networkManager.Decompress(compressedPayload);
            var menuAction = (MenuAction)menuActionInt;
            var gfPlayer = this.playerInfo.GetGFPlayer(player);

            switch (menuAction)
            {
                case MenuAction.CALL_HOUSE_VEHICLE:
                    var vehicleGuid = JsonConvert.DeserializeObject<string>(uncompressedPayload);
                    mapManager.GFPlayerCallPropertyVehicle(gfPlayer, vehicleGuid);
                    break;
            }
        }
    }
}