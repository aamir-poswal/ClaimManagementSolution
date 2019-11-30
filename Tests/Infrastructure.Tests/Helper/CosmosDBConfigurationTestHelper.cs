using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.Tests
{
    public static class CosmosDBConfigurationTestHelper
    {

        public static string CosmosDbAccount { get; set; }
        public static string CosmosDbKey { get; set; }
        public static string CosmosDbDatabaseName { get; set; }
        public static string CosmosDbContainerName { get; set; }

        static CosmosDBConfigurationTestHelper()
        {
            LoadCosmosDBConfiguration();
        }
        private static string GetKeyVaultEndpoint() => "https://ClaimKeyVault.vault.azure.net/";
        private static IConfiguration GetConfiguration()
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

        private static void LoadCosmosDBConfiguration()
        {
            IConfiguration configuration = GetConfiguration();

            CosmosDbAccount = configuration["CosmosDbAccount"];
            CosmosDbKey = configuration["CosmosDbKey"];
            CosmosDbDatabaseName = configuration["CosmosDbDatabaseName"];
            CosmosDbContainerName = configuration["CosmosDbContainerName"];

        }

    }

}
