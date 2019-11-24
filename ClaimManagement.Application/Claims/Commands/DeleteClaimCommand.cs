using ClaimManagement.Application.Claims.Events;
using ClaimManagement.Application.Common.Exceptions;
using ClaimManagement.Domain;
using ClaimManagement.Domain.Entities;
using ClaimManagement.Domain.Entities.CosmosDocument;
using ClaimManagement.Infrastructure.CosmosDB;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Application.Claims.Commands
{
    public class DeleteClaimCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }

    public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand>
    {
        private readonly ICosmosDbClaimDocumentService _cosmosDbClaimDocumentService;
        private readonly IMediator _mediator;

        public DeleteClaimCommandHandler(IMediator mediator, ICosmosDbClaimDocumentService cosmosDbClaimDocumentService)
        {
            _cosmosDbClaimDocumentService = cosmosDbClaimDocumentService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
        {
            var claimDocument = await _cosmosDbClaimDocumentService.GetItemAsync(request.Id);
            if (claimDocument == null)
            {
                throw new NotFoundException(nameof(ClaimDocument), request.Id);
            }
            
            await _cosmosDbClaimDocumentService.DeleteItemAsync(request.Id);

            var claimChangeNotification = new ClaimChangeEventNotification()
            {

                ClaimAudit = new ClaimAudit()
                {
                    Id = Convert.ToInt32(request.Id),
                    Name = claimDocument.Name,
                    Year = claimDocument.Year,
                    DamageCost = Convert.ToDecimal(claimDocument.DamageCost),
                    Type = claimDocument.Type,
                    CreatedAt = claimDocument.CreatedAt,
                    RequestName = "Delete Claim"
                }
            };
            await _mediator.Publish(claimChangeNotification);

            return Unit.Value;
        }

    }
}
