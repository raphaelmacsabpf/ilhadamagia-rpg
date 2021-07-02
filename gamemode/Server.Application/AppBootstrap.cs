using Autofac;
using CitizenFX.Core;
using Server.Application.CommandLibraries;
using Server.Application.Managers;
using Server.Application.Services;
using Server.Database;
using Server.Database.Repositories;
using Server.Domain.Interfaces;
using Server.Domain.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.Application
{
    public class AppBootstrap : BaseScript
    {
        private CommandLibraryFactory commandLibraryFactory;
        private MySqlConnectionPool mysqlConnectionPool;

        public AppBootstrap()
        {
            Stopwatch initializationStopwatch = new Stopwatch();
            initializationStopwatch.Start();
            Console.WriteLine("[IM AppBootstrap] Starting AppBootstrap");

            Console.WriteLine("\n");
            Console.WriteLine("************ Ilha da Magia [RPG v1.0a] ************");
            Console.WriteLine("→ Gamemode: IM RPG");
            Console.WriteLine("→ Criado por: Raphael Santos <raphaelmacsa@gmail.com>");
            Console.WriteLine(" ");

            Console.WriteLine("[IM AppBootstrap] Building DI Container");

            this.mysqlConnectionPool = new MySqlConnectionPool(20);
            this.commandLibraryFactory = new CommandLibraryFactory();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this.mysqlConnectionPool).As<MySqlConnectionPool>();
            builder.RegisterInstance(this.commandLibraryFactory).As<CommandLibraryFactory>();
            builder.RegisterType<AdminCommands>().As<AdminCommands>().SingleInstance();
            builder.RegisterType<ChatCommands>().As<ChatCommands>().SingleInstance();
            builder.RegisterType<HouseCommands>().As<HouseCommands>().SingleInstance();
            builder.RegisterType<MiscCommands>().As<MiscCommands>().SingleInstance();
            builder.RegisterType<MoneyCommands>().As<MoneyCommands>().SingleInstance();
            builder.RegisterType<AccountService>().As<AccountService>().SingleInstance();
            builder.RegisterType<VehicleService>().As<VehicleService>().SingleInstance();
            builder.RegisterType<PlayerService>().As<PlayerService>().SingleInstance();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();
            builder.RegisterType<HouseRepository>().As<IHouseRepository>().SingleInstance();
            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().SingleInstance();
            builder.RegisterType<OrgRepository>().As<IOrgRepository>().SingleInstance();
            builder.RegisterType<MoneyTransactionRepository>().As<IMoneyTransactionRepository>();
            builder.RegisterType<ChatManager>().As<ChatManager>().SingleInstance();
            builder.RegisterType<MapManager>().As<MapManager>().SingleInstance();
            builder.RegisterType<GameEntitiesManager>().As<GameEntitiesManager>().SingleInstance();
            builder.RegisterType<PlayerActions>().As<PlayerActions>().SingleInstance();
            builder.RegisterType<PlayerInfo>().As<PlayerInfo>().SingleInstance();
            builder.RegisterType<StateManager>().As<StateManager>().SingleInstance();
            builder.RegisterType<NetworkManager>().As<NetworkManager>().SingleInstance();
            builder.RegisterType<CommandManager>().As<CommandManager>().SingleInstance();
            builder.RegisterType<MainServer>().As<MainServer>();
            builder.RegisterType<OrgService>().As<OrgService>();
            builder.RegisterType<MoneyService>().As<MoneyService>();

            var module = new DebugResolveModule();
            builder.RegisterModule(module);
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                Console.WriteLine("[IM AppBootstrap] Resolving Scope");
                this.BuildCommandLibraryFactory(scope);
                var mainServer = scope.Resolve<MainServer>();
                var chatManager = scope.Resolve<ChatManager>();

                Console.WriteLine("[IM AppBootstrap] Registering EventHanlders");

                EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(mainServer.OnPlayerConnecting);
                EventHandlers["playerDropped"] += new Action<Player, string>(mainServer.OnPlayerDropped);
                EventHandlers["GF:Server:OnClientReady"] += new Action<Player>(mainServer.OnClientReady);
                EventHandlers["GF:Server:OnChatMessage"] += new Action<Player, string>(mainServer.OnChatMessage);
                EventHandlers["GF:Server:OnClientCommand"] += new Action<Player, string, bool, string>(mainServer.OnClientCommand);
                EventHandlers["GF:Server:OnPlayerTargetActionServerCallback"] += new Action<Player, string>(mainServer.MapManager.OnPlayerTargetActionServerCallback);
                EventHandlers["GF:Server:OnMenuAction"] += new Action<Player, int, string>(mainServer.OnPlayerMenuAction);
                EventHandlers["GF:Server:ResponseAccountSelect"] += new Action<Player, string>(mainServer.OnPlayerSelectAccount);
                EventHandlers["GF:Server:TriggerStateEvent"] += new Action<Player, string>(mainServer.OnPlayerTriggerStateEvent);
            }

            initializationStopwatch.Stop();
            Console.WriteLine($"[IM AppBootstrap] Started successful in {initializationStopwatch.ElapsedMilliseconds}ms!");
            Console.WriteLine("\n");
        }

        private void BuildCommandLibraryFactory(ILifetimeScope scope)
        {
            this.commandLibraryFactory.SetAvailableCommandLibraries(new Dictionary<Type, CommandLibrary>()
            {
                { typeof(AdminCommands), scope.Resolve<AdminCommands>() },
                { typeof(ChatCommands), scope.Resolve<ChatCommands>() },
                { typeof(HouseCommands), scope.Resolve<HouseCommands>() },
                { typeof(MiscCommands), scope.Resolve<MiscCommands>() },
                { typeof(MoneyCommands), scope.Resolve<MoneyCommands>() },
            });
        }

        private static IContainer Container { get; set; }
    }
}