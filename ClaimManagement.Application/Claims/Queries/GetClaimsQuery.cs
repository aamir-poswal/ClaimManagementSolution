using ClaimManagement.Domain.Entities;
using ClaimManagement.Domain.Entities.CosmosDocument;
using ClaimManagement.Infrastructure.CosmosDB;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Application.Claims.Queries
{
    public class GetClaimsQuery : IRequest<IEnumerable<ClaimDocument>>
    {
    }
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<ClaimDocument>>
    {
        private readonly ICosmosDbClaimDocumentService _cosmosDbClaimDocumentService;
        public GetClaimsQueryHandler(ICosmosDbClaimDocumentService cosmosDbClaimDocumentService)
        {
            _cosmosDbClaimDocumentService = cosmosDbClaimDocumentService;
        }

        public async Task<IEnumerable<ClaimDocument>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            string query = "SELECT * FROM c";
            var claims = await _cosmosDbClaimDocumentService.GetItemsAsync(query);
            return claims;
        }

    }
}
