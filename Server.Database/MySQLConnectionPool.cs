using MySqlConnector;
using System;
using System.Collections.Generic;

namespace Server.Database
{
    public class MySqlConnectionPool
    {
        private List<MySqlConnection> availableConnections;

        public MySqlConnectionPool(int poolSize)
        {
            var connectionString = "Server=192.168.99.100; Database=ilhadamagia; Uid=root; Pwd=PQPMELANCIAAZEDA;";
            this.availableConnections = new List<MySqlConnection>();

            for (int i = 0; i < poolSize; i++)
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                this.availableConnections.Add(connection);
            }
        }

        public MySqlConnection GetOne()
        {
            var random = new Random();
            var randomIndex = random.Next(this.availableConnections.Count);
            return this.availableConnections[randomIndex];
        }
    }
}