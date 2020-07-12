using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Configuration;
using System.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
namespace WebOnClouds.BotSamples
{
    public class TestCollection
    {
         [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

          [BsonElement("name")]
        public string name;

          [BsonElement("id")]
        public string id;
    }
    public class Xyz
    {
         // ADD THIS PART TO YOUR CODE


        public IMongoCollection<TestCollection> GetTasksCollectionForEdit()
        {
            string dbName="TestDB";
            string collectionName="TestCollection";
            MongoClientSettings settings = new MongoClientSettings();
            settings.RetryWrites=false;
            settings.Server = new MongoServerAddress("bot-cosmos-service-account.mongo.cosmos.azure.com", 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity("TestDB", "bot-cosmos-service-account");
            MongoIdentityEvidence evidence = new PasswordEvidence("hcbM5eDcTqVkFam46SuW4w8QqyHsBvwhmYvRVDbbTZ023EYhywMbR0I8GPeoxmNUBWgkVOiW1txxgchKFETfkQ==");

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);
            
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoTaskCollection = database.GetCollection<TestCollection>(collectionName);
            
            TestCollection collection=new TestCollection();
            collection.name="hello";
            collection.id="2";
            todoTaskCollection.InsertOne(collection);
            return todoTaskCollection;
        } 
        
           }
    public class Program
    {
                      public static void Main(string[] args)
        {
              try
    {
        Console.WriteLine("Beginning operations...\n");
        Xyz p = new Xyz();
         p.GetTasksCollectionForEdit();
       //  p.CreateDatabaseAsync();

    }
    catch (CosmosException de)
    {
        Exception baseException = de.GetBaseException();
        Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: {0}", e);
    }
    finally
    {
        Console.WriteLine("End of demo, press any key to exit.");
     //   Console.ReadKey();
    }
       
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((logging) =>
                    {
                        logging.AddDebug();
                        logging.AddConsole();
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}