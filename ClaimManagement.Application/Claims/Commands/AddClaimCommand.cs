using ClaimManagement.Application.Claims.Events;
using ClaimManagement.Domain;
using ClaimManagement.Domain.Entities;
using ClaimManagement.Domain.Entities.CosmosDocument;
using ClaimManagement.Infrastructure.CosmosDB;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ClaimManagement.Application.Claims.Commands
{

    public class AddClaimCommand : IRequest<int>
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public decimal DamageCost { get; set; }
        public ClaimType Type { get; set; }
    }

    public class AddClaimCommandValidator : AbstractValidator<AddClaimCommand>
    {
        public AddClaimCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(3, 500);
            //Validate year, it can’t be in the future and more than 10 years back.
            RuleFor(x => x.Year).NotNull().NotEmpty().InclusiveBetween(DateTime.UtcNow.AddYears(-10).Year, DateTime.UtcNow.Year);
            //Claim with DamageCost exceeding 100.000 cannot be created.
            RuleFor(x => x.DamageCost).NotNull().NotEmpty().InclusiveBetween(1M, 100.000M);
            RuleFor(x => x.Type).NotNull().NotEmpty();
        }
    }

    public class AddClaimCommandHandler : IRequestHandler<AddClaimCommand, int>
    {
        private readonly ICosmosDbClaimDocumentService _cosmosDbClaimDocumentService;
        private readonly IMediator _mediator;
        public AddClaimCommandHandler(IMediator mediator, ICosmosDbClaimDocumentService cosmosDbClaimDocumentService)
        {
            _cosmosDbClaimDocumentService = cosmosDbClaimDocumentService;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddClaimCommand request, CancellationToken cancellationToken)
        {

            var claimDocument = new ClaimDocument();
            claimDocument.Name = request.Name;
            claimDocument.Year = request.Year;
            claimDocument.DamageCost = request.DamageCost.ToString("F");
            claimDocument.Type = request.Type.GetDescription();
            claimDocument.CreatedAt = DateTime.UtcNow;

            await _cosmosDbClaimDocumentService.AddItemAsync(claimDocument);
            var claimChangeNotification = new ClaimChangeEventNotification()
            {

                ClaimAudit = new ClaimAudit()
                {
                    Id = Convert.ToInt32(claimDocument.Id),
                    Name = request.Name,
                    Year = request.Year,
                    DamageCost = request.DamageCost,
                    Type = request.Type.GetDescription(),
                    CreatedAt = claimDocument.CreatedAt,
                    RequestName = "Add Claim"
                }
            };
            await _mediator.Publish(claimChangeNotification);
            return 0;
        }
    }
}
