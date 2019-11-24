using ClaimManagement.Domain.Entities.CosmosDocument;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.CosmosDB
{
    public interface ICosmosDbClaimDocumentService
    {
        Task<IEnumerable<ClaimDocument>> GetItemsAsync(string query);
        Task<ClaimDocument> GetItemAsync(string id);
        Task AddItemAsync(ClaimDocument claimDocument);
        Task UpdateItemAsync(string id, ClaimDocument claimDocument);
        Task DeleteItemAsync(string id);
    }
}
