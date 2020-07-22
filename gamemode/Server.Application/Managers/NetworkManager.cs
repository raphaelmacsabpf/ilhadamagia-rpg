using CitizenFX.Core;
using Shared.CrossCutting;

namespace Server.Application.Managers
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