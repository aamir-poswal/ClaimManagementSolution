using ClaimManagement.Domain.Entities.CosmosDocument;
using ClaimManagement.Infrastructure.CosmosDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Linq;
using Newtonsoft.Json;

namespace ClaimManagement.Infrastructure.CosmosDB
{
    public class CosmosDbClaimDocumentService : ICosmosDbClaimDocumentService
    {
        private Container _container;

        public CosmosDbClaimDocumentService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(ClaimDocument claimDocument)
        {
            var query = "SELECT  * FROM Claims c order by c._ts desc";
            var claims = await GetItemsAsync(query);
            var id = 1;
            if (claims.Count() > 0)
            {
                id = Convert.ToInt32(claims.FirstOrDefault().Id) + 1;
            }
            claimDocument.Id = id.ToString();
            await this._container.CreateItemAsync<ClaimDocument>(claimDocument, new PartitionKey(claimDocument.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<ClaimDocument>(id, new PartitionKey(id));
        }

        public async Task<ClaimDocument> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<ClaimDocument> response = await this._container.ReadItemAsync<ClaimDocument>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ClaimDocument>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<ClaimDocument>(new QueryDefinition(queryString));
            List<ClaimDocument> results = new List<ClaimDocument>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, ClaimDocument claimDocument)
        {
            await this._container.UpsertItemAsync<ClaimDocument>(claimDocument, new PartitionKey(id));
        }
    }
}
