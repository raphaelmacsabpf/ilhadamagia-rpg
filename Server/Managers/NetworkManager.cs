using CitizenFX.Core;
using GF.CrossCutting;

namespace Server.Managers
{
    public class NetworkManager
    {
        public NetworkManager()
        {
        }

        public void SendPayloadToPlayer(Player player, PayloadType payloadType, string jsonPayload)
        {
            player.TriggerEvent("GF:Client:SendPayload", (int)payloadType, jsonPayload);
        }
    }
}