using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Enums;
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
            EventHandlers["Chat:GF:Client:OnClientText"] += new Action<string>(mainClient.OnClientText);

            // TODO: Remover comando Info
            EventHandlers["client:Client:Info"] += new Action(mainClient.OnInfo);

            // GF Events
            RegisterScriptEventHandler(ScriptEvent.SendClientMessage, new Action<int, string>(mainClient.GFSendClientMessage));
            RegisterScriptEventHandler(ScriptEvent.SpawnPlayer, new Action<string, float, float, float, float, bool>(mainClient.GFSpawnPlayer));
            RegisterScriptEventHandler(ScriptEvent.Kill, new Action(mainClient.GFKill));
            RegisterScriptEventHandler(ScriptEvent.SetPlayerPos, new Action<Vector3>(mainClient.GFSetPlayerPos));
            RegisterScriptEventHandler(ScriptEvent.TeleportPlayerToPosition, new Action<Vector3, int>(mainClient.GFTeleportPlayerToPosition));
            RegisterScriptEventHandler(ScriptEvent.SetPedHealth, new Action<int>(mainClient.GFSetPedHealth));
            RegisterScriptEventHandler(ScriptEvent.SetPedArmour, new Action<int>(mainClient.GFSetPedArmour));
            RegisterScriptEventHandler(ScriptEvent.GiveWeaponToPed, new Action<uint, int, bool, bool>(mainClient.GFGiveWeaponToPed));
            RegisterScriptEventHandler(ScriptEvent.SendPayload, new Action<int, string, int>(mainClient.OnPayloadReceive));
            RegisterScriptEventHandler(ScriptEvent.SetPlayerMoney, new Action<int>(mainClient.GFSetPlayerMoney));
            RegisterScriptEventHandler(ScriptEvent.CreateVehicle, new Action<uint, int, int, int, float, float, float, float>(mainClient.CreateVehicle));
            RegisterScriptEventHandler(ScriptEvent.DeleteVehicle, new Action<int>(mainClient.DeleteVehicle));
            RegisterScriptEventHandler(ScriptEvent.OpenMenu, new Action<int, string, int>(menuManager.OpenMenu));
            RegisterScriptEventHandler(ScriptEvent.OpenNUIView, new Action<int, bool, string, int>(mainClient.OpenNUIView));
            RegisterScriptEventHandler(ScriptEvent.CloseNUIView, new Action<int, bool>(mainClient.CloseNUIView)); // TODO: Mudar de close para Hide (faz mais sentido)
            RegisterScriptEventHandler(ScriptEvent.CreatePlayerVehicle, new Action<uint>(mainClient.CreatePlayerVehicle));
            RegisterScriptEventHandler(ScriptEvent.SwitchOutPlayer, new Action(mainClient.SwitchOutPlayer));
            RegisterScriptEventHandler(ScriptEvent.SwitchInPlayer, new Action<float, float, float>(mainClient.SwitchInPlayer));
            RegisterScriptEventHandler(ScriptEvent.SyncPlayerDateTime, new Action<string>(mainClient.SyncPlayerDateTime));
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

        private void RegisterScriptEventHandler(ScriptEvent scriptEvent, Delegate @delegate)
        {
            EventHandlers[scriptEvent.ToString()] += @delegate;
        }
    }
}