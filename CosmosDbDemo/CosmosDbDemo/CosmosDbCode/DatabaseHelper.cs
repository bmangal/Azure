using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CosmosDbDemo
{
    /// <summary>
    /// Database helper class used to get list of, create, and delete database.
    /// </summary>
	public static class DatabaseHelper
	{

        /// <summary>
        /// Get list of all databases in a Cosmos DB account.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        public static void ViewDatabasesSy(string serviceEndpoint, string authKey)
		{
			Console.WriteLine("\nVIEW ALL DATABASES\n");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var dbs = client.CreateDatabaseQuery().ToList();
                foreach (var db in dbs)
                {
                    Console.WriteLine($" Database Id: {db.Id}, Resource Id: {db.ResourceId}");
                }

                Console.WriteLine($"\nTotal databases: {dbs.Count}");
            }
		}

        /// <summary>
        /// Create a new database in a Cosmos DB account.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="dbToCeate">Database Name</param>
        /// <returns>Task</returns>
		public async static Task CreateDatabaseAsy(string serviceEndpoint, string authKey, string dbToCeate)
        {
			Console.WriteLine($"\nCREATE NEW DATABASE {dbToCeate}");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var dbDefinition = new Database { Id = dbToCeate };
                var result = await client.CreateDatabaseAsync(dbDefinition);
                var db = result.Resource;

                Console.WriteLine($"Database Id: {db.Id}, Resource Id: {db.ResourceId}");
            }
		}

        /// <summary>
        /// Delete existing database by name.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <returns>Task</returns>
        public async static Task DeleteDatabaseAsy(string serviceEndpoint, string authKey, string db)
        {
            Console.WriteLine($"\nDELETE DATABASE {db}");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var dbUri = UriFactory.CreateDatabaseUri(db);
                await client.DeleteDatabaseAsync(dbUri);
            }
        }

	}
}
