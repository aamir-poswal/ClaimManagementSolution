using ClaimManagement.Application.Claims.Commands;
using ClaimManagement.Application.Common.Exceptions;
using ClaimManagement.Domain;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Appication.Tests.Commands
{
    public class ClaimCommandsTest
    {
        [Test]
        public async Task Test_AddClaimCommand_ShouldAddClaim()
        {
            var service = await TestHelper.GetCosmosDbClaimDocumentService();

            AddClaimCommand command = new AddClaimCommand()
            {
                Name = "New Application Test Claim",
                Year = 2014,
                DamageCost = 13.1M,
                Type = ClaimType.Fire
            };
            var fakeMediator = new Mock<IMediator>();

            var handler = new AddClaimCommandHandler(fakeMediator.Object, service);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.GreaterOrEqual(result, 0);
        }

        [Test]
        public async Task Test_DeleteClaimCommand_ShouldRemoveClaim()
        {
            try
            {
                var service = await TestHelper.GetCosmosDbClaimDocumentService();

                DeleteClaimCommand command = new DeleteClaimCommand() { Id = "3" };

                var fakeMediator = new Mock<IMediator>();

                var handler = new DeleteClaimCommandHandler(fakeMediator.Object, service);

                var result = await handler.Handle(command, CancellationToken.None);

                Assert.NotNull(result);

            }
            catch (NotFoundException notFoundException)
            {
                Assert.Fail($"No claim found {notFoundException}");
            }
        }

    }
}
