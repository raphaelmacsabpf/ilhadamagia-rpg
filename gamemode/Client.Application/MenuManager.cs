using CitizenFX.Core;
using Shared.CrossCutting.Converters;
using Shared.CrossCutting.Dto;
using Shared.CrossCutting.Enums;
using MenuAPI;
using Newtonsoft.Json;
using Shared.CrossCutting;
using System.Collections.Generic;
using GF.CrossCutting.Enums;
using static MenuAPI.Menu;
using GF.CrossCutting.Dto.MenuActions;

namespace Client.Application
{
    public class MenuManager : BaseClientScript
    {
        private readonly ClientNetworkManager clientNetworkManager;

        public MenuManager(ClientNetworkManager clientNetworkManager)
        {
            this.clientNetworkManager = clientNetworkManager;
        }

        public void OpenMenu(int menuTypeInt, string compressedJsonPayload, int uncompressedLength)
        {
            // Prevent menu opening by 'M' key
            MenuController.MenuToggleKey = (Control)(-1);
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

            var jsonPayload = clientNetworkManager.Decompress(compressedJsonPayload, uncompressedLength);
            var menuType = (MenuType)menuTypeInt;
            switch (menuType)
            {
                case MenuType.House: OpenHouseMenu(jsonPayload); break;
                case MenuType.Org: OpenOrgMenu(jsonPayload); break;
                case MenuType.Ammunation: OpenAmmunationMenu(jsonPayload); break;
                case MenuType.GasStation: OpenGasStationMenu(jsonPayload); break;
            }
        }

        private void OpenHouseMenu(string jsonPayload)
        {
            var vehicleList = JsonConvert.DeserializeObject<List<VehicleDto>>(jsonPayload);

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
                var vehicleHash = (GameVehicleHash)vehicle.Hash;
                var vehicleName = VehicleConverter.GetVehicleName(vehicleHash);
                var menuItem = new MenuItem($"#{index} {vehicleName}", $"Combustível: {vehicle.Fuel}%, Conservação: {vehicle.EngineHealth}%");
                menuItem.ItemData = new CallHouseVehicleDto(vehicle.Guid);
                vehicleListMenu.AddMenuItem(menuItem);
            }
            MenuController.AddSubmenu(menu, vehicleListMenu);
            MenuController.BindMenuItem(menu, vehicleListMenu, callHouseVehicleMenu);
            var vehicleListMenuItems = vehicleListMenu.GetMenuItems();
            vehicleListMenu.OnItemSelect += VehicleListMenu_OnItemSelect;

            menu.OpenMenu();
        }

        private void OpenOrgMenu(string jsonPayload)
        {
            var orgData = JsonConvert.DeserializeObject<OrgDataDto>(jsonPayload);
            Menu menu = new Menu("Organização", orgData.Name);
            MenuController.AddMenu(menu);

            var membersMenuItem = new MenuItem("Membros");
            menu.AddMenuItem(membersMenuItem);

            var equipMenuItem = new MenuItem("Equipar");
            menu.AddMenuItem(equipMenuItem);
            menu.OnItemSelect += new ItemSelectEvent((_menu, _menuItem, _index) =>
            {
                if(_menuItem == equipMenuItem)
                {
                    TriggerMenuAction(MenuAction.ORG_EQUIP, null);
                }
            });

            var membersMenu = new Menu("Membros");
            foreach (var member in orgData.Members)
            {
                membersMenu.AddMenuItem(new MenuItem(member.Username, $"Cargo: {member.Role}") { ItemData = new object[] { member }, LeftIcon = MenuItem.Icon.INFO});
            }

            MenuController.AddSubmenu(menu, membersMenu);
            MenuController.BindMenuItem(menu, membersMenu, membersMenuItem);

            menu.OpenMenu();
        }

        private void Menu_OnItemSelect(Menu menu, MenuItem menuItem, int itemIndex)
        {
            throw new System.NotImplementedException();
        }

        private void OpenAmmunationMenu(string jsonPayload)
        {
            var ammunationName = JsonConvert.DeserializeObject<string>(jsonPayload);
            Menu menu = new Menu("Ammunation", ammunationName);
            MenuController.AddMenu(menu);

            var pistols = new MenuItem("Pistolas");
            menu.AddMenuItem(pistols);

            var pistolsMenu = new Menu("Pistolas");
            pistolsMenu.AddMenuItem(new MenuItem("AP Pistol", "18 balas") { ItemData = new object[] { GameWeaponHash.APPistol, 18 }, LeftIcon = MenuItem.Icon.GUN });
            pistolsMenu.AddMenuItem(new MenuItem("Heavy Pistol", "18 balas") { ItemData = new object[] { GameWeaponHash.HeavyPistol, 18 }, LeftIcon = MenuItem.Icon.GUN });
            pistolsMenu.AddMenuItem(new MenuItem("Heavy Revolver", "6 balas") { ItemData = new object[] { GameWeaponHash.Revolver, 6 }, LeftIcon = MenuItem.Icon.GUN });
            MenuController.AddSubmenu(menu, pistolsMenu);
            MenuController.BindMenuItem(menu, pistolsMenu, pistols);

            menu.OpenMenu();
        }

        private void OpenGasStationMenu(string jsonPayload)
        {
            var gasStationName = JsonConvert.DeserializeObject<string>(jsonPayload);
            Menu menu = new Menu("Posto de Gasolina", gasStationName);
            MenuController.AddMenu(menu);

            menu.AddMenuItem(new MenuItem("Abastecer 25%", "Por $1000") { ItemData = new object[] { 25 }, LeftIcon = MenuItem.Icon.INFO });
            menu.AddMenuItem(new MenuItem("Abastecer 50%", "Por $1500") { ItemData = new object[] { 50 }, LeftIcon = MenuItem.Icon.INFO });
            menu.AddMenuItem(new MenuItem("Abastecer 75%", "Por $2000") { ItemData = new object[] { 75 }, LeftIcon = MenuItem.Icon.INFO });
            menu.AddMenuItem(new MenuItem("Abastecer 100%", "Por $2500") { ItemData = new object[] { 100 }, LeftIcon = MenuItem.Icon.INFO });

            menu.OpenMenu();
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
            CallServerAction(ServerEvent.OnMenuAction, (int)menuAction, compressedPayload);
        }
    }
}