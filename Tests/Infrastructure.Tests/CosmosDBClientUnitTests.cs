using ClaimManagement.Domain.Entities.CosmosDocument;
using Microsoft.Azure.Cosmos;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.Tests
{
    public class CosmosDBClientUnitTests
    {

        private readonly CosmosClient CosmosClient = TestHelper.GetCosmosClient();

        [Test]
        public async Task Test_CreateDatabaseAsync_ShouldCreateDatabaseIfNotExist()
        {
            var database = await CosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDBConfigurationTestHelper.CosmosDbDatabaseName);

            Assert.IsNotNull(database);

            Assert.True(database.StatusCode == System.Net.HttpStatusCode.OK);

        }

        [Test]
        public async Task Test_CreateContainerAsync_ShouldCreateContainerIfNotExist()
        {
            var database = await CosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDBConfigurationTestHelper.CosmosDbDatabaseName);

            var container = await database.Database.CreateContainerIfNotExistsAsync(CosmosDBConfigurationTestHelper.CosmosDbContainerName, "/id");

            Assert.IsNotNull(container);

            Assert.True(container.StatusCode == System.Net.HttpStatusCode.OK);

        }

        [Test]
        public async Task Test_GetItemByIdAsync_ShouldReturnResultIfExist()
        {
            try
            {
                var database = await CosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDBConfigurationTestHelper.CosmosDbDatabaseName);

                Container container = await database.Database.CreateContainerIfNotExistsAsync(CosmosDBConfigurationTestHelper.CosmosDbContainerName, "/id");

                var id = "8";

                ItemResponse<ClaimDocument> response = await container.ReadItemAsync<ClaimDocument>(id, new PartitionKey(id));

                Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK );
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Assert.True(ex.StatusCode == System.Net.HttpStatusCode.NotFound);
            }

        }

    }
}
