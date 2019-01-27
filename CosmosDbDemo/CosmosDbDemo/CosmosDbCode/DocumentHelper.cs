using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbDemo
{
    /// <summary>
    /// Document helper class used to get list of, create, and delete documents.
    /// </summary>
	public static class DocumentHelper
	{
        /// <summary>
        /// Create new documents in existing collection.
        /// Creating documents using dynamic object, JSON, and POCO object.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <returns>Task</returns>
		public async static Task CreateDocumentsAsy(string serviceEndpoint, string authKey, string db, string coll)
		{
			Console.WriteLine($"\nCREATE DOCUMENTS IN DATABASE {db} COLLECTION {coll}\n");

            Uri collUri = UriFactory.CreateDocumentCollectionUri(db, coll);

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                dynamic p1Dynamic = new { id = "1", name = "Chris Doe", zipcode = "02142", state = "MA" };
                Document doc1 = await CreateDocumentAsy(client, collUri, p1Dynamic);

                var p2Json = @"{ ""name"": ""John Doe"", ""zipcode"": ""01805"", ""state"": ""MA"" }";
                var doc2Obj = JsonConvert.DeserializeObject(p2Json);
                Document doc2 = await CreateDocumentAsy(client, collUri, doc2Obj);

                var p3Poco = new Person { Name = "Jane Doe", Zipcode = "03060", State="nh" };
                Document doc3 = await CreateDocumentAsy(client, collUri, p3Poco);
            }
		}

        /// <summary>
        /// Create a new document based on document object.
        /// </summary>
        /// <param name="client">Document Client</param>
        /// <param name="collUri">Collection URI</param>
        /// <param name="docToCreate">Object with properties representing document</param>
        /// <returns>Task with Document</returns>
        private async static Task<Document> CreateDocumentAsy(DocumentClient client, Uri collUri, object docToCreate)
        {
            var result = await client.CreateDocumentAsync(collUri, docToCreate);
            var doc = result.Resource;
            Console.WriteLine($"Created new document Id: {doc.Id}, {doc}");
            return result;
        }

        /// <summary>
        /// Get list of all documents in a collection using SQL query.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
		public static void QueryDocumentsWithSqlSy(string serviceEndpoint, string authKey, string db, string coll)
		{
			Console.WriteLine($"\nQUERY DOCUMENTS (SQL) IN DATABASE {db} COLLECTION {coll}\n");

            Uri collUri = UriFactory.CreateDocumentCollectionUri(db, coll);

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var sql = "SELECT * FROM c";
                var options = new FeedOptions { EnableCrossPartitionQuery = true };

                Console.WriteLine("Querying for dynamic objects");
                var docs = client.CreateDocumentQuery(collUri, sql, options).ToList();
                foreach (var doc in docs)
                {
                    Console.WriteLine($"Id: {doc.id}, Name: {doc.name}, State: {doc.state}, Zipcode: {doc.zipcode}");
                }

                Console.WriteLine("\nQuerying for defined type Person");
                var people = client.CreateDocumentQuery<Person>(collUri, sql, options).ToList();
                foreach (var person in people)
                {
                    Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, State: {person.State}, Zipcode: {person.Zipcode}");
                }
            }
			Console.WriteLine();
        }

        /// <summary>
        /// Get Documents using Paging.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <param name="pageSize">Number of documents in one page</param>
        /// <returns>Task</returns>
		public async static Task QueryDocumentsWithPagingAsy(string serviceEndpoint, string authKey, string db, string coll, int pageSize)
		{
			Console.WriteLine($"\nQUERY DOCUMENTS (PAGED RESULTS) IN DATABASE {db} COLLECTION {coll}");

            Uri collUri = UriFactory.CreateDocumentCollectionUri(db, coll);

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var sql = "SELECT * FROM c";
                var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = pageSize };

                var query = client.CreateDocumentQuery(collUri, sql, options).AsDocumentQuery();

                int pageNum = 0;
                // Looping thourgh Pages
                while (query.HasMoreResults)
                {
                    pageNum++;
                    Console.WriteLine($"\nPage {pageNum}\n");
                    var docs = await query.ExecuteNextAsync();
                    // Looping through Documents in a Page
                    foreach (var doc in docs)
                    {
                        Console.WriteLine($"Id: {doc.id}, Name: {doc.name}, State: {doc.state},  Zipcode: {doc.zipcode}");
                    }
                }
            }
			Console.WriteLine();
		}

        /// <summary>
        /// Delete all of the documents in a collection.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <returns>Task</returns>
		public async static Task DeleteDocumentsAsy(string serviceEndpoint, string authKey, string db, string coll)
		{
			Console.WriteLine($"\nDELETE DOCUMENTS FROM DATABASE {db} COLLECTION {coll}\n");

            Uri collUri = UriFactory.CreateDocumentCollectionUri(db, coll);

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

                // We need partition key to delete
                var sql = "SELECT c._self, c.state FROM c";
                var docKeys = client.CreateDocumentQuery(collUri, sql, feedOptions).ToList();

                foreach (var docKey in docKeys)
                {
                    var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(docKey.state) };
                    await client.DeleteDocumentAsync(docKey._self, requestOptions);
                }

                Console.WriteLine($"Deleted {docKeys.Count} documents\n");
            }
		}

	}
}
