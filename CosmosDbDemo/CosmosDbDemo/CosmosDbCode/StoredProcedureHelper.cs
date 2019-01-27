using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbDemo
{
    /// <summary>
    /// Function helper class used to execute server side code - stored prcoedures.
    /// The value of partition key needs to be passed to all of the stored procedures at the time of execution.
    /// </summary>
    public static class StoredProcedureHelper
    {
        /// <summary>
        /// Create a new document in existing collection by calling stored procedure.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <returns>Task</returns>
        public async static Task CreateDocumentAsy(string serviceEndpoint, string authKey, string db, string coll)
        {
            Console.WriteLine($"\nBY STORED PROCEDURE CREATE DOCUMENT IN DATABASE {db} COLLECTION {coll}\n");

            Uri spUri = UriFactory.CreateStoredProcedureUri(db, coll, "addDocumentProc");
            var p = new Person { Id = "4", Name = "Mary Doe", Zipcode = "06001", State = "CT" };
            var requestOptions = new RequestOptions { PartitionKey = new PartitionKey(p.State) };

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var result = await client.ExecuteStoredProcedureAsync<Document>(spUri, requestOptions, p);
                var doc = result.Response;
                Console.WriteLine($"Created new document Id: {doc.Id}, {doc}");
            }
        }


        /// <summary>
        /// Get list of all documents by calling stored procedure.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        /// <returns>Task</returns>
        public async static Task QueryDocumentsAsy(string serviceEndpoint, string authKey, string db, string coll)
        {
            Console.WriteLine($"\nBY STORED PROCEDURE QUERY DOCUMENTS IN DATABASE {db} COLLECTION {coll}\n");

            Uri spUri = UriFactory.CreateStoredProcedureUri(db, coll, "getAllDocumentsProc");
            var requestOptions = new RequestOptions { PartitionKey = new PartitionKey("MA") };

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                var result = await client.ExecuteStoredProcedureAsync<List<Document>>(spUri, requestOptions);
                var docs = result.Response;
                foreach (var doc in docs)
                {
                    Console.WriteLine($"Document Id: {doc.Id}, {doc}");
                }
            }
        }


    }
}
