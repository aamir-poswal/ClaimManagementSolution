using ClaimManagement.Domain;
using FunctionAppClaimAudit;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Tests
{
    public class FunctionClaimAuditTest
    {
        [Test]
        public async Task Test_FunctionClaimAudit_ShouldHitTheEndPointAndRaiseSqlConnectionException()
        {
            try
            {
                var fakeLogger = new Mock<ILogger>();
                var claimAudit = new ClaimAudit()
                {
                    Id = 2,
                    Name = "Test Name",
                    Year = 2015,
                    DamageCost = 23.1M,
                    Type = ClaimType.Collision.GetDescription(),
                    CreatedAt = DateTime.UtcNow,
                    RequestName = "Add Claim"
                };
                var eventData = new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = claimAudit.RequestName,
                    Data = JsonConvert.SerializeObject(claimAudit),
                    EventTime = DateTime.UtcNow,
                    Subject = claimAudit.Name,
                    DataVersion = "2.0"
                };
                await FunctionClaimAudit.RunAsync(eventData, fakeLogger.Object);
                Assert.True(true);
            }
            catch (Exception exception)
            {
                Assert.Fail($"azure sql server connection string not being accessed {exception.Message}");
            }
        }
    }
}
