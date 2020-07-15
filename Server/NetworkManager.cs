using CitizenFX.Core;
using GF.CrossCutting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
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
