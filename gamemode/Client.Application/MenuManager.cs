using CitizenFX.Core;
using GF.CrossCutting;
using GF.CrossCutting.Dto;
using MenuAPI;
using Newtonsoft.Json;
using Shared.CrossCuting.Domain.Enums;
using Shared.CrossCutting;
using System.Collections.Generic;

namespace Client.Application
{
    public class MenuManager : BaseScript
    {
        private readonly ClientNetworkManager clientNetworkManager;

        public MenuManager(ClientNetworkManager clientNetworkManager)
        {
            this.clientNetworkManager = clientNetworkManager;
        }

        public void OpenMenu(int menuTypeInt, string compressedJsonPayload, int uncompressedLength)
        {
            var jsonPayload = clientNetworkManager.Decompress(compressedJsonPayload, uncompressedLength);
            var menuType = (MenuType)menuTypeInt;
            switch (menuType)
            {
                case MenuType.House: OpenHouseMenu(jsonPayload); break;
            }
        }

        private void OpenHouseMenu(string jsonPayload)
        {
            var vehicleList = JsonConvert.DeserializeObject<List<VehicleDto>>(jsonPayload);
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

            Menu menu = new Menu("Menu - Casa", "Gerencie sua proriedade");
            MenuController.AddMenu(menu);
            var callHouseVehicleMenu = new MenuItem("Chamar veículo", "Utilize essa opção para chamar o veículo da sua casa!")
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.TICK
            };

            menu.AddMenuItem(callHouseVehicleMenu);
            Menu vehicleListMenu = new Menu("Escolha um carro", "subtitulo legalzinho"); // TODO: Arrumar o subtitulo
            int index = 0;
            foreach (var vehicle in vehicleList)
            {
                index++;
                var vehicleHash = (DomainVehicleHash)vehicle.Hash;
                var vehicleName = VehicleConvert.GetVehicleName(vehicleHash);
                var menuItem = new MenuItem($"#{index} {vehicleName}", $"Combustível: {vehicle.Fuel}%, Conservação: {vehicle.EngineHealth}%");
                menuItem.ItemData = vehicle.Guid;
                vehicleListMenu.AddMenuItem(menuItem);
            }
            MenuController.AddSubmenu(menu, vehicleListMenu);
            MenuController.BindMenuItem(menu, vehicleListMenu, callHouseVehicleMenu);
            var vehicleListMenuItems = vehicleListMenu.GetMenuItems();
            vehicleListMenu.OnItemSelect += VehicleListMenu_OnItemSelect;

            menu.OpenMenu();

            // Prevent menu opening by 'M' key
            MenuController.MenuToggleKey = (Control)(-1);
        }

        private void VehicleListMenu_OnItemSelect(Menu menu, MenuItem menuItem, int itemIndex)
        {
            TriggerMenuAction(MenuAction.CALL_HOUSE_VEHICLE, menuItem.ItemData);
            menu.CloseMenu();
        }

        private void TriggerMenuAction(MenuAction menuAction, object payload)
        {
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var compressedPayload = clientNetworkManager.Compress(jsonPayload);
            TriggerServerEvent("GF:Server:OnMenuAction", (int)menuAction, compressedPayload);
        }
    }
}