using CitizenFX.Core;
using MenuAPI;
using Shared.CrossCutting;

namespace Client.Application
{
    public class MenuManager : BaseScript
    {
        public MenuManager(bool ignoreFiveMInitialization)
        {
        }

        public void OpenMenu(int menuTypeInt)
        {
            var menuType = (MenuType)menuTypeInt;
            switch (menuType)
            {
                case MenuType.House: OpenHouseMenu(); break;
            }
        }

        private void OpenHouseMenu()
        {
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

            Menu menu = new Menu("Menu - Casa", "Gerencie sua proriedade");
            MenuController.AddMenu(menu);
            var callHouseVehicle = new MenuItem("Chamar veículo", "Utilize essa opção para chamar o veículo da sua casa!")
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.TICK
            };

            menu.AddMenuItem(callHouseVehicle);
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item == callHouseVehicle)
                {
                    TriggerMenuAction(MenuAction.CALL_HOUSE_VEHICLE);
                    menu.CloseMenu();
                }
            };

            menu.OpenMenu();

            // Prevent menu opening by 'M' key
            MenuController.MenuToggleKey = (Control)(-1);
        }

        private void TriggerMenuAction(MenuAction menuAction)
        {
            TriggerServerEvent("GF:Server:OnMenuAction", (int)menuAction);
        }
    }
}