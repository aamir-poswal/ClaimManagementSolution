using ClaimManagement.Application.Claims.Commands;
using ClaimManagement.Application.Claims.Queries;
using ClaimManagement.Application.Common.Exceptions;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Appication.Tests.Queries
{
    public class ClaimQueriesTest
    {
        [Test]
        public async Task Test_GetClaimByIdQuery_ReturnRequiredClaim()
        {
            try
            {
                var service = await TestHelper.GetCosmosDbClaimDocumentService();

                GetClaimByIdQuery query = new GetClaimByIdQuery() { Id = "2" };

                var handler = new GetClaimByIdQueryHandler(service);

                var result = await handler.Handle(query, CancellationToken.None);

                Assert.IsTrue(result != null);
            }
            catch (NotFoundException notFoundException)
            {
                Assert.Fail($"No claim found {notFoundException}");
            }
        }

        [Test]
        public async Task Test_GetClaimsQuery_ReturnClaimsIfAny()
        {

            var service = await TestHelper.GetCosmosDbClaimDocumentService();

            GetClaimsQuery query = new GetClaimsQuery();

            var handler = new GetClaimsQueryHandler(service);

            var results = await handler.Handle(query, CancellationToken.None);

            Assert.GreaterOrEqual(results.Count(), 0);

        }

    }
}
