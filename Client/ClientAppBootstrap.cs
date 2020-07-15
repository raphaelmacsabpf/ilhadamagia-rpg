using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientAppBootstrap : BaseScript
    {
        public ClientAppBootstrap()
        {
            var playerInfo = new PlayerInfo();
            var markersManager = new MarkersManager();
            var drawTextAPI = new DrawTextAPI(true);
            var render = new Render(drawTextAPI, markersManager);
            var playerActions = new PlayerActions(true);
            var targetsManager = new TargetsManager(playerActions);
            var mainClient = new MainClient(playerInfo, markersManager, render, playerActions, targetsManager);

            this.Tick += render.RenderTickHandler;
            this.Tick += targetsManager.TargetsTickHandler;

            // Default Events
            EventHandlers["baseevents:onPlayerDied"] += new Action<int, dynamic>(mainClient.OnDie);
            EventHandlers["playerSpawned"] += new Action<dynamic>(mainClient.OnPlayerSpawn);
            EventHandlers["onClientMapStart"] += new Action(mainClient.OnPlayerMapStart);
            EventHandlers["onClientResourceStart"] += new Action<string>(mainClient.OnClientResourceStart);

            // Commands
            EventHandlers["client:Client:Info"] += new Action(mainClient.OnInfo);
            EventHandlers["client:Client:Veh"] += new Action(mainClient.onVeh);

            // GF Events
            EventHandlers["GF:Client:SendClientMessage"] += new Action<int, string>(mainClient.GFSendClientMessage);
            EventHandlers["GF:Client:SpawnPlayer"] += new Action<string, float, float, float, float>(mainClient.GFSpawnPlayer);
            EventHandlers["GF:Client:Kill"] += new Action(mainClient.GFKill);
            EventHandlers["GF:Client:SetPlayerPos"] += new Action<Vector3>(mainClient.GFSetPlayerPos);
            EventHandlers["GF:Client:SetPedArmour"] += new Action<int>(mainClient.GFSetPedArmour);
            EventHandlers["GF:Client:GiveWeaponToPed"] += new Action<uint, int, bool, bool>(mainClient.GFGiveWeaponToPed);
            EventHandlers["Chat:GF:Client:OnClientText"] += new Action<string>(mainClient.OnClienText);
            EventHandlers["GF:Client:SendPayload"] += new Action<int, string>(mainClient.OnPayloadReceive);
            EventHandlers["GF:Client:SetPlayerMoney"] += new Action<int>(mainClient.GFSetPlayerMoney);
            EventHandlers["GF:Client:DeleteVehicle"] += new Action<int>(mainClient.GFDeleteVehicle);
        }
    }
}
