﻿using Autofac;
using CitizenFX.Core;
using Server.Application.Managers;
using Server.Database;
using System;
using System.Diagnostics;
using Server.Application.Services;

namespace Server.Application
{
    public class AppBootstrap : BaseScript
    {
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

            var builder = new ContainerBuilder();
            builder.RegisterInstance(new MySqlConnectionPool(20)).As<MySqlConnectionPool>();
            builder.RegisterType<AccountRepository>().As<AccountRepository>().SingleInstance();
            builder.RegisterType<HouseRepository>().As<HouseRepository>().SingleInstance();
            builder.RegisterType<VehicleRepository>().As<VehicleRepository>().SingleInstance();
            builder.RegisterType<OrgRepository>().As<OrgRepository>().SingleInstance();
            builder.RegisterType<MoneyTransactionRepository>().As<MoneyTransactionRepository>();
            builder.RegisterType<MenuManager>().As<MenuManager>().SingleInstance();
            builder.RegisterType<ChatManager>().As<ChatManager>().SingleInstance();
            builder.RegisterType<MapManager>().As<MapManager>().SingleInstance();
            builder.RegisterType<GameEntitiesManager>().As<GameEntitiesManager>().SingleInstance();
            builder.RegisterType<PlayerActions>().As<PlayerActions>().SingleInstance();
            builder.RegisterType<PlayerInfo>().As<PlayerInfo>().SingleInstance();
            builder.RegisterType<StateManager>().As<StateManager>().SingleInstance();
            builder.RegisterType<NetworkManager>().As<NetworkManager>().SingleInstance();
            builder.RegisterType<CommandManager>().As<CommandManager>().SingleInstance();
            builder.RegisterType<MainServer>().As<MainServer>();
            builder.RegisterType<MoneyService>().As<MoneyService>();

            var module = new DebugResolveModule();
            builder.RegisterModule(module);
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                Console.WriteLine("[IM AppBootstrap] Resolving Scope");
                var mainServer = scope.Resolve<MainServer>();
                var chatManager = scope.Resolve<ChatManager>();
                var menuManager = scope.Resolve<MenuManager>();

                Console.WriteLine("[IM AppBootstrap] Registering EventHanlders");

                EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(mainServer.OnPlayerConnecting);
                EventHandlers["playerDropped"] += new Action<Player, string>(mainServer.OnPlayerDropped);
                EventHandlers["GF:Server:OnClientReady"] += new Action<Player>(mainServer.OnClientReady);
                EventHandlers["GF:Server:OnChatMessage"] += new Action<Player, string>(chatManager.OnChatMessage);
                EventHandlers["GF:Server:OnClientCommand"] += new Action<Player, int, bool, string>(mainServer.CommandManager.OnClientCommand);
                EventHandlers["GF:Server:OnPlayerTargetActionServerCallback"] += new Action<Player, string>(mainServer.MapManager.OnPlayerTargetActionServerCallback);
                EventHandlers["GF:Server:OnMenuAction"] += new Action<Player, int, string>(menuManager.OnPlayerMenuAction);
                EventHandlers["GF:Server:ResponseAccountSelect"] += new Action<Player, string>(mainServer.OnPlayerSelectAccount);
                EventHandlers["GF:Server:TriggerStateEvent"] += new Action<Player, string>(mainServer.OnPlayerTriggerStateEvent);
            }

            initializationStopwatch.Stop();
            Console.WriteLine($"[IM AppBootstrap] Started successful in {initializationStopwatch.ElapsedMilliseconds}ms!");
            Console.WriteLine("\n");
        }

        private static IContainer Container { get; set; }
    }
}