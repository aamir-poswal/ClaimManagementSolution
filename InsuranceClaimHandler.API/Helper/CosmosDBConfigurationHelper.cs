using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceClaimHandler.API
{
    public static class CosmosDBConfigurationHelper
    {
        public static CosmosDBConfiguration LoadCosmosDBConfiguration(IConfiguration configuration)
        {
            CosmosDBConfiguration cosmosDBConfiguration = new CosmosDBConfiguration();
            cosmosDBConfiguration.CosmosDbAccount = configuration["CosmosDbAccount"];
            cosmosDBConfiguration.CosmosDbKey = configuration["CosmosDbKey"];
            cosmosDBConfiguration.CosmosDbDatabaseName = configuration["CosmosDbDatabaseName"];
            cosmosDBConfiguration.CosmosDbContainerName = configuration["CosmosDbContainerName"];
            return cosmosDBConfiguration;
        }

    }

    public class CosmosDBConfiguration
    {
        public  string CosmosDbAccount { get; set; }
        public  string CosmosDbKey { get; set; }
        public  string CosmosDbDatabaseName { get; set; }
        public  string CosmosDbContainerName { get; set; }
    }

}
