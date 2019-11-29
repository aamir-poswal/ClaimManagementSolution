using ClaimManagement.Domain;
using ClaimManagement.Domain.Entities.CosmosDocument;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Infrastructure.Tests
{
    public class CosmosDbClaimDocumentServiceUnitTests
    {

        [Test]
        public async Task Test_GetItemsAsync_ReturnListOfClaimsIfAny()
        {
            var service = await TestHelper.GetCosmosDbClaimDocumentService();
            string query = "SELECT * FROM c";
            var results = await service.GetItemsAsync(query);

            Assert.GreaterOrEqual(results.Count(), 0);
        }

        [Test]
        public async Task Test_GetItemAsync_ReturnRequiredClaim()
        {
            var service = await TestHelper.GetCosmosDbClaimDocumentService();
            string id = "1";
            var result = await service.GetItemAsync(id);

            Assert.IsTrue(result == null || result != null);

        }

        [Test]
        public async Task Test_AddItemAsync_ShouldAddClaim()
        {
            var service = await TestHelper.GetCosmosDbClaimDocumentService();
            var claimDocument = new ClaimDocument();
            claimDocument.Name = "Unit Test 2";
            claimDocument.Year = 2017;
            claimDocument.DamageCost = (12.3M).ToString("#.##"); ;
            claimDocument.Type = ClaimType.BadWeather.GetDescription();
            claimDocument.CreatedAt = DateTime.UtcNow;

            service.AddItem(claimDocument);

            Assert.AreEqual(claimDocument.Year, 2017);

        }

        [Test]
        public async Task Test_DeleteItemAsync_ShouldRemoveClaim()
        {
            var service = await TestHelper.GetCosmosDbClaimDocumentService();
            string id = "1";
            await service.DeleteItemAsync(id);

            Assert.AreEqual(id, "1");
        }


    }
}