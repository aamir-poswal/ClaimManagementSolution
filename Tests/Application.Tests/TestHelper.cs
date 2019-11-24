using ClaimManagement.Application;
using ClaimManagement.Infrastructure.CosmosDB;
using ClaimManagement.Infrastructure.CosmosDB.Persistence;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.Appication.Tests
{
    public static class TestHelper
    {
        private static string GetKeyVaultEndpoint() => "https://ClaimKeyVault.vault.azure.net/";

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder();
            var keyVaultEndpoint = GetKeyVaultEndpoint();
            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                   new KeyVaultClient.AuthenticationCallback(
                      azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.AddAzureKeyVault(
                   keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            }
            var configurationRoot = builder.Build();

            return configurationRoot;
        }

        public static async Task<CosmosDbClaimDocumentService> GetCosmosDbClaimDocumentService()
        {
            var configurationRoot = GetConfiguration();
            var cosmosDbAccount = configurationRoot["CosmosDbAccount"];
            var cosmosDbKey = configurationRoot["CosmosDbKey"];
            var cosmosDbDatabaseName = configurationRoot["CosmosDbDatabaseName"];
            var cosmosDbContainerName = configurationRoot["CosmosDbContainerName"];
            var service = await CosmosDBContext.InitializeCosmosClientInstanceAsync(cosmosDbAccount, cosmosDbKey, cosmosDbDatabaseName, cosmosDbContainerName);
            return service;
        }

        public static IPublishEvent GetPublishEvent(IConfiguration configuration)
        {
            return new PublishEvent(configuration);
        }
    }
}
