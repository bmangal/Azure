using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbDemo
{
    class Program
    {
        /// <summary>
        /// Printing out list of available input options for user.
        /// </summary>
        private static void DisplayInputOptions()
        {
            
            Console.WriteLine("Microsoft Azure Cosmos DB Demo with SQL API and .NET SDK Client Code");
            Console.WriteLine("");
            Console.WriteLine("  #  Demo");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("  1  View Databases");
            Console.WriteLine("  2  Create Database & Collection");
            Console.WriteLine("  3  View Collections in a Database");
            Console.WriteLine("  4  Create Documents");
            Console.WriteLine("  5  View Documents with SQL");
            Console.WriteLine("  6  View Documents with Paging");
            Console.WriteLine("  7  Stored Procedure Create Document");
            Console.WriteLine("  8  Stored Procedure View Documents by Partition Key");
            Console.WriteLine("  9  User Defined Functions");
            Console.WriteLine(" 10  Clean Up");
            Console.WriteLine("\n  E  Exit");

        }

        private static void Main(string[] args)
        {
            // Reading configurtion settings of database account from App.config file.
            string endpoint = ConfigurationManager.AppSettings["ServiceEndPoint"];
            string masterKey = ConfigurationManager.AppSettings["AuthKey"];

            //Default values of database and collection name
            string database = "DataFestDB";
            string collection = "BostonDataFestColl";

            Task.Run(async () =>
            {
                DisplayInputOptions();
                while (true)
                {
                    Console.WriteLine("");
                    Console.Write("Please Enter Demo #: ");
                    var input = Console.ReadLine();
                    var demoId = input.ToUpper().Trim();
                    if (demoId == "E")
                    {
                        break;
                    }
                    else
                    {
                        if (demoId == "2")
                        {
                            Console.WriteLine("");
                            Console.Write("Enter Database Name: ");
                            input = Console.ReadLine();
                            database = input.Trim();
                            Console.WriteLine("");
                            Console.Write("Enter Collection Name: ");
                            input = Console.ReadLine();
                            collection = input.Trim();      
                        }
                        try
                        {
                            Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine();
                            Console.Write($"Results from Demo # {demoId}");
                            Console.WriteLine();
                            switch (demoId)
                            {
                                case "1":
                                    // View list of all databases in a database account.
                                    DatabaseHelper.ViewDatabasesSy(endpoint, masterKey);
                                    break;
                                case "2":
                                    // Create a new database.
                                    await DatabaseHelper.CreateDatabaseAsy(endpoint, masterKey, database);
                                    // Create a new collection in database with partition key.
                                    await CollectionHelper.CreateCollectionAsy(endpoint, masterKey, database, collection, "/state");
                                    break;
                                case "3":
                                    // View list of all collections in a database.
                                    CollectionHelper.ViewCollectionsSy(endpoint, masterKey, database);
                                    break;
                                case "4":
                                    // Create new documents.
                                    await DocumentHelper.CreateDocumentsAsy(endpoint, masterKey, database, collection);
                                    break;
                                case "5":
                                    // View list of all documents.
                                    DocumentHelper.QueryDocumentsWithSqlSy(endpoint, masterKey, database, collection);
                                    break;
                                case "6":
                                    // View documents by paging with given page size.
                                    await DocumentHelper.QueryDocumentsWithPagingAsy(endpoint, masterKey, database, collection, 2);
                                    break;
                                case "7": 
                                    // Using Stored Procedure to create new document.
                                    await StoredProcedureHelper.CreateDocumentAsy(endpoint, masterKey, database, collection);
                                    break;
                                case "8":
                                    // Using Stored Procedure to get list of all documents.
                                    await StoredProcedureHelper.QueryDocumentsAsy(endpoint, masterKey, database, collection);
                                    break;
                                case "9":
                                    // Using User Defined Function to change few properties to upper case.
                                    FunctionHelper.QueryDocuments(endpoint, masterKey, database, collection);
                                    break;

                                case "10":
                                    // Clean up step to delete all created documents, collection, and database.
                                    await DocumentHelper.DeleteDocumentsAsy(endpoint, masterKey, database, collection);
                                    await CollectionHelper.DeleteCollectionAsy(endpoint, masterKey, database, collection);
                                    await DatabaseHelper.DeleteDatabaseAsy(endpoint, masterKey, database);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            var message = ex.Message;
                            while (ex.InnerException != null)
                            {
                                message += Environment.NewLine + ex.InnerException.Message;
                            }
                            Console.WriteLine($"Error: {message}");
                        }
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey(true);
                        Console.Clear();
                        DisplayInputOptions();
                    }
                }
            }).Wait();
        }

    }
}
