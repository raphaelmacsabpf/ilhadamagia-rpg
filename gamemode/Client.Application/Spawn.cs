using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace Client.Application
{
    public class Spawn : BaseClientScript
    {
        private static bool _spawnLock = false;

        public Spawn()
        {
            Debug.WriteLine("Script Spawn started");
        }

        public static void FreezePlayer(int playerId, bool freeze)
        {
            var ped = GetPlayerPed(playerId);

            SetPlayerControl(playerId, !freeze, 0);

            if (!freeze)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);

                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);

                FreezeEntityPosition(ped, false);
                //SetCharNeverTargetted(ped, false)
                SetPlayerInvincible(playerId, false);
            }
            else
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);

                SetEntityCollision(ped, false, true);
                FreezeEntityPosition(ped, true);
                //SetCharNeverTargetted(ped, true)
                SetPlayerInvincible(playerId, true);

                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }

        public static async Task SpawnPlayer(string skin, float x, float y, float z, float heading, bool fastSpawn) // TODO: 02/09/2020 Rever fastspawn após switchin
        {
            if (_spawnLock)
                return;

            _spawnLock = true;

            FreezePlayer(PlayerId(), true);
            var skinHashKey = GetHashKey(skin);
            var pedModel = Convert.ToUInt32(skinHashKey);
            RequestModel(pedModel);
            while (HasModelLoaded(pedModel) == false)
            {
                await Delay(fastSpawn ? 1 : 300);
            }
            await Game.Player.ChangeModel(skinHashKey);

            SetPedDefaultComponentVariation(GetPlayerPed(-1));
            RequestCollisionAtCoord(x, y, z);

            var ped = GetPlayerPed(-1);

            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            ClearPedTasksImmediately(ped);
            RemoveAllPedWeapons(ped, false);
            ClearPlayerWantedLevel(PlayerId());

            while (!HasCollisionLoadedAroundEntity(ped))
            {
                await Delay(1);
            }

            ShutdownLoadingScreen();

            FreezePlayer(PlayerId(), false);
            _spawnLock = false;
        }
    }
}