using CitizenFX.Core;
using System;

namespace Client.Application
{
    public class ClientAppBootstrap : BaseScript
    {
        public ClientAppBootstrap()
        {
            var menuManager = new MenuManager(true);
            var playerInfo = new PlayerInfo();
            var markersManager = new MarkersManager();
            var drawTextAPI = new DrawTextAPI(true);
            var playerActions = new PlayerActions(true);
            var targetsManager = new TargetsManager(playerActions);
            var render = new Render(drawTextAPI, markersManager, targetsManager);
            var mainClient = new MainClient(playerInfo, markersManager, render, playerActions, targetsManager, menuManager);

            this.Tick += render.RenderTickHandler;
            this.Tick += targetsManager.TargetsTickHandler;

            // Default Events
            EventHandlers["baseevents:onPlayerDied"] += new Action<int, dynamic>(mainClient.OnDie);
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
            EventHandlers["GF:Client:TeleportPlayerToPosition"] += new Action<Vector3, int>(mainClient.GFTeleportPlayerToPosition);
            EventHandlers["GF:Client:SetPedArmour"] += new Action<int>(mainClient.GFSetPedArmour);
            EventHandlers["GF:Client:GiveWeaponToPed"] += new Action<uint, int, bool, bool>(mainClient.GFGiveWeaponToPed);
            EventHandlers["Chat:GF:Client:OnClientText"] += new Action<string>(mainClient.OnClienText);
            EventHandlers["GF:Client:SendPayload"] += new Action<int, string, int>(mainClient.OnPayloadReceive);
            EventHandlers["GF:Client:SetPlayerMoney"] += new Action<int>(mainClient.GFSetPlayerMoney);
            EventHandlers["GF:Client:DeleteVehicle"] += new Action<int>(mainClient.GFDeleteVehicle);
            EventHandlers["GF:Client:OpenMenu"] += new Action<int>(menuManager.OpenMenu);
        }
    }
}