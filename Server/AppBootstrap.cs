﻿using Autofac;
using CitizenFX.Core;
using System;
using System.Diagnostics;

namespace Server
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
            ChatManager chatManager = new ChatManager(true);
            PlayerActions playerActions = new PlayerActions(true);

            var builder = new ContainerBuilder();

            builder.RegisterInstance(chatManager).As<ChatManager>();
            builder.RegisterType<MapManager>().As<MapManager>().SingleInstance();
            builder.RegisterInstance(playerActions).As<PlayerActions>();
            builder.RegisterType<PlayerInfo>().As<PlayerInfo>().SingleInstance();

            builder.RegisterType<NetworkManager>().As<NetworkManager>().SingleInstance();
            builder.RegisterType<CommandManager>().As<CommandManager>().SingleInstance();
            builder.RegisterType<MainServer>().As<MainServer>();

            var module = new DebugResolveModule();
            builder.RegisterModule(module);
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                Console.WriteLine("[IM AppBootstrap] Resolving Scope");
                var mainServer = scope.Resolve<MainServer>();

                Console.WriteLine("[IM AppBootstrap] Registering EventHanlders");

                EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(mainServer.OnPlayerConnecting);
                EventHandlers["GF:Server:OnClientReady"] += new Action<Player>(mainServer.OnClientReady);
                EventHandlers["GF:Server:OnChatMessage"] += new Action<Player, string>(chatManager.OnChatMessage);
                EventHandlers["GF:Server:OnClientCommand"] += new Action<Player, int, bool, string>(mainServer.CommandManager.OnClientCommand);
                EventHandlers["GF:Server:CreatedVehicle"] += new Action<Player, int>(mainServer.MapManager.PlayerCreatedVehicle);
                EventHandlers["GF:Server:OnPlayerTargetActionServerCallback"] += new Action<Player, string, string>(mainServer.MapManager.OnPlayerTargetActionServerCallback);
            }

            initializationStopwatch.Stop();
            Console.WriteLine($"[IM AppBootstrap] Started successful in {initializationStopwatch.ElapsedMilliseconds}ms!");
            Console.WriteLine("\n");
        }

        private static IContainer Container { get; set; }
    }
}