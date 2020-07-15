﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using GF.CrossCutting.Dto;
using System;
using System.Threading.Tasks;

namespace Client
{
    public class Render : BaseScript
    {
        private int moneyUpdateRate;
        private bool updatingMoney;
        private int lastDisplayedMoney;
        private int lastMoneyValue;
        private bool moneyStateRaisingUp;
        private readonly DrawTextAPI drawTextAPI;
        private readonly MarkersManager markersManager;
        private readonly TargetsManager targetsManager;
        private bool inputEventsSemaphoreEnable;

        public Render(DrawTextAPI drawTextAPI, MarkersManager markersManager, TargetsManager targetsManager)
        {
            this.drawTextAPI = drawTextAPI;
            this.markersManager = markersManager;
            this.targetsManager = targetsManager;
            this.moneyUpdateRate = 1;
            this.lastDisplayedMoney = 1;
            this.lastMoneyValue = 1;
            this.inputEventsSemaphoreEnable = true;
        }

        public async Task RenderTickHandler()
        {
            while (true)
            {
                await Delay(1);
                RenderPlayerMoney();
                RenderMarkers();
                if (this.inputEventsSemaphoreEnable && API.IsControlPressed(0, 46)) // Key E
                {
                    this.inputEventsSemaphoreEnable = false;
                    this.targetsManager.OnInteractionKeyPressed();
                    await Task.Factory.StartNew(async () =>
                    {
                        await Delay(1000);
                        this.inputEventsSemaphoreEnable = true;
                    });
                }
            }
        }

        public void UpdateMoney(int oldMoney, int newMoney)
        {
            if (oldMoney == newMoney)
            {
                return;
            }

            if (newMoney > oldMoney)
            {
                this.moneyUpdateRate = (Math.Abs(newMoney - oldMoney) / 60);
                this.moneyStateRaisingUp = true;
            }
            else
            {
                this.moneyUpdateRate = (Math.Abs(oldMoney - newMoney) / 60);
                this.moneyStateRaisingUp = false;
            }
            if (this.moneyUpdateRate <= 0)
            {
                this.moneyUpdateRate = 1;
            }

            this.lastMoneyValue = newMoney;
            this.updatingMoney = true;
        }

        private void RenderPlayerMoney() // TODO: Melhorar este método, TA HORRÍVEL
        {
            var moneyToDisplay = this.lastMoneyValue;
            if (updatingMoney)
            {
                if (this.moneyStateRaisingUp && this.lastDisplayedMoney < this.lastMoneyValue)
                {
                    moneyToDisplay = this.lastDisplayedMoney + this.moneyUpdateRate;
                    this.lastDisplayedMoney = moneyToDisplay;
                }
                else if (this.moneyStateRaisingUp == false && this.lastDisplayedMoney > this.lastMoneyValue)
                {
                    moneyToDisplay = this.lastDisplayedMoney - this.moneyUpdateRate;
                    this.lastDisplayedMoney = moneyToDisplay;
                }
                else
                {
                    this.updatingMoney = false;
                    this.lastDisplayedMoney = this.lastMoneyValue;
                }
            }
            else
            {
                this.lastDisplayedMoney = this.lastMoneyValue;
            }

            int r, g, b;
            if (moneyToDisplay >= 0)
            {
                r = 110;
                g = 194;
                b = 110;
            }
            else
            {
                r = 255;
                g = 102;
                b = 85;
            }

            int x = 0, y = 0;
            API.GetActiveScreenResolution(ref x, ref y);
            this.drawTextAPI.DrawText($"${moneyToDisplay}", x - 10, 58f, 0.6f, r, g, b, 255, 7, 2, true, true, 0, 0, 0);
        }

        private void RenderMarkers()
        {
            foreach (MarkerDto markerdto in this.markersManager.Markers)
            {
                API.DrawMarker(markerdto.type, markerdto.posX, markerdto.posY, markerdto.posZ, markerdto.dirX, markerdto.dirY, markerdto.dirZ, markerdto.rotX, markerdto.rotY, markerdto.rotZ, markerdto.scaleX, markerdto.scaleY, markerdto.scaleZ, markerdto.red, markerdto.green, markerdto.blue, markerdto.alpha, markerdto.bobUpAndDown, markerdto.faceCamera, markerdto.p19, markerdto.rotate, markerdto.textureDict, markerdto.textureName, markerdto.drawOnEnts);
            }
        }
    }
}