using ClaimManagement.Application.Common.Exceptions;
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
    public class GetClaimByIdQuery : IRequest<ClaimDocument>
    {
        public string Id { get; set; }
    }
    public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, ClaimDocument>
    {
        private readonly ICosmosDbClaimDocumentService _cosmosDbClaimDocumentService;
        public GetClaimByIdQueryHandler(ICosmosDbClaimDocumentService cosmosDbClaimDocumentService)
        {
            _cosmosDbClaimDocumentService = cosmosDbClaimDocumentService;
        }
        public async Task<ClaimDocument> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
        {
            var claim = await _cosmosDbClaimDocumentService.GetItemAsync(request.Id);
            if (claim == null)
            {
                throw new NotFoundException(nameof(ClaimDocument), request.Id);
            }
            return claim;
        }
    }
}
