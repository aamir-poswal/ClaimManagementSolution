using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.CosmosDB.Persistence
{
    public static class CosmosDBContext
    {
        public static async Task<CosmosDbClaimDocumentService> InitializeCosmosClientInstanceAsync(string account, string key, string databaseName, string containerName)
        {
            
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
            CosmosClient client = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();

            CosmosDbClaimDocumentService cosmosDbService = new CosmosDbClaimDocumentService(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}
