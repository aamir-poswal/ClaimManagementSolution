using ClaimManagement.Infrastructure.CosmosDB;
using ClaimManagement.Infrastructure.CosmosDB.Persistence;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.Tests
{
    public static class TestHelper
    {


        public static async Task<CosmosDbClaimDocumentService> GetCosmosDbClaimDocumentService()
        {
            var cosmosDbAccount = CosmosDBConfigurationTestHelper.CosmosDbAccount;
            var cosmosDbKey = CosmosDBConfigurationTestHelper.CosmosDbKey;
            var cosmosDbDatabaseName = CosmosDBConfigurationTestHelper.CosmosDbDatabaseName;
            var cosmosDbContainerName = CosmosDBConfigurationTestHelper.CosmosDbContainerName;
            var service = await CosmosDBContext.InitializeCosmosClientInstanceAsync(cosmosDbAccount, cosmosDbKey, cosmosDbDatabaseName, cosmosDbContainerName);
            return service;
        }

        public static CosmosClient GetCosmosClient()
        {
            var cosmosDbAccount = CosmosDBConfigurationTestHelper.CosmosDbAccount;
            var cosmosDbKey = CosmosDBConfigurationTestHelper.CosmosDbKey;
            var cosmosClient = new CosmosClient(cosmosDbAccount, cosmosDbKey);

            return cosmosClient;
        }

    }
}
