using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Shared.CrossCutting;
using System;
using System.Threading.Tasks;

namespace Client.Application
{
    public class PlayerActions : BaseClientScript
    {
        public PlayerActions(bool ignoreFiveMInitialization)
        {
        }

        public void PushNotification(string message, int periodInMs)
        {
            var notification = Screen.ShowNotification(message);
            Task.Factory.StartNew(async () =>
            {
                await Delay(periodInMs);
                notification.Hide();
            });
        }

        public void SendMessageToPlayerChat(ChatColor chatColor, string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            TriggerEvent("chat:addMessage", new
            {
                color = ChatColorValue.GetRGBA(chatColor),
                args = new[] { $"[{timestamp}] {message}" }
            });
        }

        public void Kill()
        {
            Game.PlayerPed.Kill();
        }

        public void SetPlayerPos(Vector3 targetPosition)
        {
            Game.PlayerPed.Position = targetPosition;
        }

        public async void TeleportPlayerToPosition(Vector3 targetPosition, int transitionDurationInMs = 500)
        {
            API.SetPlayerControl(API.PlayerId(), false, 0);
            API.DoScreenFadeOut(100);
            while (API.IsScreenFadingOut())
            {
                await Delay(16);
            }
            Game.PlayerPed.Position = targetPosition;
            await Delay(transitionDurationInMs);
            API.DoScreenFadeIn(100);
            API.SetPlayerControl(API.PlayerId(), true, 0);
        }

        internal void GivePlayerWeapon(WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)weaponHash, ammoCount, isHidden, equipNow);
        }

        internal void SetPlayerHealth(int value)
        {
            API.SetEntityHealth(Game.PlayerPed.Handle, value);
        }

        internal void SetPlayerArmour(int value)
        {
            API.SetPedArmour(Game.PlayerPed.Handle, value);
        }

        internal async void SpawnPlayer(string skinName, float x, float y, float z, float heading, bool fastSpawn)
        {
            API.SetTimeScale(1f);
            await Spawn.SpawnPlayer(skinName, x, y, z, heading, fastSpawn);
        }
    }
}