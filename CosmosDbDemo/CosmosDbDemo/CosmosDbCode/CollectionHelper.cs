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
    /// Collection helper class used to get list of, create, and delete collection.
    /// </summary>
	public static class CollectionHelper
	{
        /// <summary>
        /// Get list of all collections in a Cosmos database.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        public static void ViewCollectionsSy(string serviceEndpoint, string authKey, string db)
        {
            Console.WriteLine($"\nVIEW ALL COLLECTIONS IN DATABASE {db}");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                Uri databaseUri = UriFactory.CreateDatabaseUri(db);

                var collections = client.CreateDocumentCollectionQuery(databaseUri).ToList();

                foreach (var collection in collections)
                {
                    Console.WriteLine($"Collection ID: {collection.Id}");
                }

                Console.WriteLine($"\nTotal collections in {db} database: {collections.Count}");
            }
        }

        /// <summary>
        /// Create a new collection in existing database. Define collection's partition
        /// key and reserverd throughput in RU/s.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="collToCreate">Collection Name</param>
        /// <param name="partitionKey">Path of Partition Key</param>
        /// <param name="throughputRUs">Reserved Throughput in RU/s. 
        /// Minimum: 400, Range: (400 - 1,000,000), Default: 20000
        /// </param>
        /// <returns>Task</returns>
        public async static Task CreateCollectionAsy(string serviceEndpoint, string authKey, 
            string db, string collToCreate, string partitionKey, int throughputRUs = 400)
		{
			Console.WriteLine($"\nCREATE COLLECTION {collToCreate} IN DATABASE {db}\n");
			Console.WriteLine($"Throughput: {throughputRUs} RU/sec");
			Console.WriteLine($"Partition key: {partitionKey}\n");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                Uri dbUri = UriFactory.CreateDatabaseUri(db);

                var partitionKeyDefinition = new PartitionKeyDefinition();
                // Note: Only One Partition Key
                partitionKeyDefinition.Paths.Add(partitionKey);

                var collDefinition = new DocumentCollection
                {
                    Id = collToCreate,
                    PartitionKey = partitionKeyDefinition
                };
                var options = new RequestOptions { OfferThroughput = throughputRUs };

                var result = await client.CreateDocumentCollectionAsync(dbUri, collDefinition, options);
                var coll = result.Resource;

                Console.WriteLine($"Created new collection ID: {coll.Id}");
            }
        }

        /// <summary>
        /// Delete a collection.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <returns>Task</returns>
        public async static Task DeleteCollectionAsy(string serviceEndpoint, string authKey, string db, string coll)
		{
			Console.WriteLine($"\nDELETE COLLECTION {coll} IN DATABASE {db}");

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var collUri = UriFactory.CreateDocumentCollectionUri(db, coll);
                await client.DeleteDocumentCollectionAsync(collUri);

                Console.WriteLine($"Deleted collection {coll} from database {db}");
            }
		}

	}
}
