﻿using CitizenFX.Core;
using LZ4;
using Server.Application.Entities;
using Shared.CrossCutting;
using System;
using System.Text;

namespace Server.Application.Managers
{
    public class NetworkManager
    {
        public NetworkManager()
        {
        }

        public void SendPayloadToPlayer(Player player, PayloadType payloadType, string jsonPayload)
        {
            var compressedJsonPayload = Compress(jsonPayload);
            player.TriggerEvent("GF:Client:SendPayload", (int)payloadType, compressedJsonPayload, jsonPayload.Length);
        }

        public string Compress(string text)
        {
            var compressed = Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(text)));
            return compressed;
        }

        public string Decompress(string compressed)
        {
            var uncompressed = Encoding.UTF8.GetString(LZ4Codec.Unwrap(Convert.FromBase64String(compressed)));
            return uncompressed;
        }

        public void SyncPlayerDateTime(GFPlayer gfPlayer)
        {
            gfPlayer.Player.TriggerEvent("GF:Client:SyncPlayerDateTime", DateTime.Now.ToString());
        }
    }
}