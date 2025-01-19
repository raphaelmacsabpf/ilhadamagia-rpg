using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Application
{
    public class ClientAppBootstrap : BaseClientScript
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
            EventHandlers["Chat:GF:Client:OnClientText"] += new Action<string>(mainClient.OnClientText);

            // TODO: Remover comando Info
            EventHandlers["client:Client:Info"] += new Action(mainClient.OnInfo);

            // GF Events
            RegisterClientEventHandler(ClientEvent.SendClientMessage, new Action<int, string>(mainClient.GFSendClientMessage));
            RegisterClientEventHandler(ClientEvent.SpawnPlayer, new Action<string, float, float, float, float, bool>(mainClient.GFSpawnPlayer));
            RegisterClientEventHandler(ClientEvent.Kill, new Action(mainClient.GFKill));
            RegisterClientEventHandler(ClientEvent.SetPlayerPos, new Action<Vector3>(mainClient.GFSetPlayerPos));
            RegisterClientEventHandler(ClientEvent.TeleportPlayerToPosition, new Action<Vector3, int>(mainClient.GFTeleportPlayerToPosition));
            RegisterClientEventHandler(ClientEvent.SetPedHealth, new Action<int>(mainClient.GFSetPedHealth));
            RegisterClientEventHandler(ClientEvent.SetPedArmour, new Action<int>(mainClient.GFSetPedArmour));
            RegisterClientEventHandler(ClientEvent.GiveWeaponToPed, new Action<uint, int, bool, bool>(mainClient.GFGiveWeaponToPed));
            RegisterClientEventHandler(ClientEvent.SendPayload, new Action<int, string, int>(mainClient.OnPayloadReceive));
            RegisterClientEventHandler(ClientEvent.SetPlayerMoney, new Action<int>(mainClient.GFSetPlayerMoney));
            RegisterClientEventHandler(ClientEvent.CreateVehicle, new Action<uint, int, int, int, float, float, float, float>(mainClient.CreateVehicle));
            RegisterClientEventHandler(ClientEvent.DeleteVehicle, new Action<int>(mainClient.DeleteVehicle));
            RegisterClientEventHandler(ClientEvent.OpenMenu, new Action<int, string, int>(menuManager.OpenMenu));
            RegisterClientEventHandler(ClientEvent.OpenNUIView, new Action<int, bool, string, int>(mainClient.OpenNUIView));
            RegisterClientEventHandler(ClientEvent.CloseNUIView, new Action<int, bool>(mainClient.CloseNUIView)); // TODO: Mudar de close para Hide (faz mais sentido)
            RegisterClientEventHandler(ClientEvent.CreatePlayerVehicle, new Action<uint>(mainClient.CreatePlayerVehicle));
            RegisterClientEventHandler(ClientEvent.SwitchOutPlayer, new Action(mainClient.SwitchOutPlayer));
            RegisterClientEventHandler(ClientEvent.SwitchInPlayer, new Action<float, float, float>(mainClient.SwitchInPlayer));
            RegisterClientEventHandler(ClientEvent.SyncPlayerDateTime, new Action<string, int>(mainClient.SyncPlayerDateTime));
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

        private void RegisterClientEventHandler(ClientEvent clientEvent, Delegate @delegate)
        {
            EventHandlers[clientEvent.ToString()] += @delegate;
        }
    }
}