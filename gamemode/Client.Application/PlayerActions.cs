﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Shared.CrossCutting;
using System;
using System.Threading.Tasks;

namespace Client.Application
{
    public class PlayerActions : BaseScript
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

        internal void GivePlayerWeapon(WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
        {
            API.GiveWeaponToPed(Game.PlayerPed.Handle, (uint)weaponHash, ammoCount, isHidden, equipNow);
        }

        internal void SetPlayerArmour(int value)
        {
            API.SetPedArmour(Game.PlayerPed.Handle, value);
        }

        internal async void SpawnPlayer(string skinName, float x, float y, float z, float heading)
        {
            await Spawn.SpawnPlayer(skinName, x, y, z, heading);
        }
    }
}