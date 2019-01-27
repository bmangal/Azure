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
    /// Function helper class used to execute server side code - user defined functions.
    /// </summary>
    public static class FunctionHelper
    {
        /// <summary>
        /// Get list of documents using SQL Query.
        /// Change case of few properties by calling user defined function.
        /// </summary>
        /// <param name="serviceEndpoint">Cosmos DB Service End Point</param>
        /// <param name="authKey">Cosmos DB Authentication Key</param>
        /// <param name="db">Database Name</param>
        /// <param name="coll">Collection Name</param>
        public static void QueryDocuments(string serviceEndpoint, string authKey, string db, string coll)
        {
            Console.WriteLine($"\nQUERY DOCUMENTS (SQL) IN DATABASE {db} COLLECTION {coll}\n");

            Uri collUri = UriFactory.CreateDocumentCollectionUri(db, coll);

            using (var client = new DocumentClient(new Uri(serviceEndpoint), authKey))
            {
                //Prepend udf. before function name
                var sql = "SELECT udf.upperCaseFunc(c.name) AS name, c.zipcode, udf.upperCaseFunc(c.state) AS state FROM c";
                var options = new FeedOptions { EnableCrossPartitionQuery = true };

                var docs = client.CreateDocumentQuery(collUri, sql, options).ToList();
                foreach (var doc in docs)
                {
                    Console.WriteLine($"Name: {doc.name}, ZipCode: {doc.zipcode}, State: {doc.state}");
                }
            }
            Console.WriteLine();
        }


    }
}
