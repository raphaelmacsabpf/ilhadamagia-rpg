﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Application
{
    public class ClientAppBootstrap : BaseScript
    {
        private bool firstTick = false;
        private MainClient mainClientHandler;

        public ClientAppBootstrap()
        {
            var playerInfo = new PlayerInfo();
            var markersManager = new MarkersManager();
            var drawTextAPI = new DrawTextAPI(true);
            var playerActions = new PlayerActions(true);
            var targetsManager = new TargetsManager(playerActions);
            var render = new Render(drawTextAPI, markersManager, targetsManager);
            var clientNetworkManager = new ClientNetworkManager();
            var menuManager = new MenuManager(clientNetworkManager);
            var mainClient = new MainClient(playerInfo, markersManager, render, playerActions, targetsManager, clientNetworkManager);

            this.Tick += render.RenderTickHandler;
            this.Tick += targetsManager.TargetsTickHandler;
            this.Tick += mainClient.Wait1SecondTickHandler;
            this.Tick += mainClient.PlayerStateTickHandler;
            this.Tick += OnTick;

            // Default Events
            EventHandlers["baseevents:onPlayerDied"] += new Action<int, dynamic>(mainClient.OnDie);
            EventHandlers["onClientMapStart"] += new Action(mainClient.OnPlayerMapStart);
            EventHandlers["onClientResourceStart"] += new Action<string>(mainClient.OnClientResourceStart);

            // TODO: Remover comando Info
            EventHandlers["client:Client:Info"] += new Action(mainClient.OnInfo);

            // GF Events
            EventHandlers["GF:Client:SendClientMessage"] += new Action<int, string>(mainClient.GFSendClientMessage);
            EventHandlers["GF:Client:SpawnPlayer"] += new Action<string, float, float, float, float, bool>(mainClient.GFSpawnPlayer);
            EventHandlers["GF:Client:Kill"] += new Action(mainClient.GFKill);
            EventHandlers["GF:Client:SetPlayerPos"] += new Action<Vector3>(mainClient.GFSetPlayerPos);
            EventHandlers["GF:Client:TeleportPlayerToPosition"] += new Action<Vector3, int>(mainClient.GFTeleportPlayerToPosition);
            EventHandlers["GF:Client:SetPedHealth"] += new Action<int>(mainClient.GFSetPedHealth);
            EventHandlers["GF:Client:SetPedArmour"] += new Action<int>(mainClient.GFSetPedArmour);
            EventHandlers["GF:Client:GiveWeaponToPed"] += new Action<uint, int, bool, bool>(mainClient.GFGiveWeaponToPed);
            EventHandlers["Chat:GF:Client:OnClientText"] += new Action<string>(mainClient.OnClienText);
            EventHandlers["GF:Client:SendPayload"] += new Action<int, string, int>(mainClient.OnPayloadReceive);
            EventHandlers["GF:Client:SetPlayerMoney"] += new Action<int>(mainClient.GFSetPlayerMoney);
            EventHandlers["GF:Client:CreateVehicle"] += new Action<uint, int, int, int, float, float, float, float>(mainClient.CreateVehicle);
            EventHandlers["GF:Client:DeleteVehicle"] += new Action<int>(mainClient.DeleteVehicle);
            EventHandlers["GF:Client:OpenMenu"] += new Action<int, string, int>(menuManager.OpenMenu);
            EventHandlers["GF:Client:OpenNUIView"] += new Action<int, bool, string, int>(mainClient.OpenNUIView);
            EventHandlers["GF:Client:CloseNUIView"] += new Action<int, bool>(mainClient.CloseNUIView); // TODO: Mudar de close para Hide (faz mais sentido)
            EventHandlers["GF:Client:CreatePlayerVehicle"] += new Action<uint>(mainClient.CreatePlayerVehicle);
            EventHandlers["GF:Client:SwitchOutPlayer"] += new Action(mainClient.SwitchOutPlayer);
            EventHandlers["GF:Client:SwitchInPlayer"] += new Action<float, float, float>(mainClient.SwitchInPlayer);
            EventHandlers["GF:Client:SyncPlayerDateTime"] += new Action<string>(mainClient.SyncPlayerDateTime);
            mainClientHandler = mainClient;
        }

        private async Task OnTick()
        {
            if (!firstTick)
            {
                firstTick = true;
                API.RegisterNuiCallbackType("NUI_ENDPOINT");
                EventHandlers["__cfx_nui:NUI_ENDPOINT"] += new Action<IDictionary<string, object>, CallbackDelegate>(mainClientHandler.OnNuiEndpointCall);
            }
        }
    }
}