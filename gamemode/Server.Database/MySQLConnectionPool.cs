﻿using MySqlConnector;
using System;
using System.Collections.Generic;

namespace Server.Database
{
    public class MySqlConnectionPool
    {
        private List<MySqlConnection> availableConnections;

        public MySqlConnectionPool(int poolSize)
        {
            Console.WriteLine($"[IM MySqlConnectionPool] Starting database connection pool with size: {poolSize}");
            var connectionString = "Server=192.168.99.100; Database=ilhadamagia; Uid=root; Pwd=PQPMELANCIAAZEDA;";
            this.availableConnections = new List<MySqlConnection>();
            for (int i = 0; i < poolSize; i++)
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                this.availableConnections.Add(connection);
                Console.WriteLine($"[IM MySqlConnectionPool] Started pool connection #{i}");
            }
            Console.WriteLine($"[IM MySqlConnectionPool] Database connection pool started successful with size: {poolSize}");
        }

        public MySqlConnection GetOne()
        {
            var random = new Random();
            var randomIndex = random.Next(this.availableConnections.Count);
            Console.WriteLine($"[IM MySqlConnectionPool] Database connection in use: #{randomIndex}");
            return this.availableConnections[randomIndex];
        }
    }
}