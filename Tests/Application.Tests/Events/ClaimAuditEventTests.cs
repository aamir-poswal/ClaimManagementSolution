using ClaimManagement.Application.Claims.Events;
using ClaimManagement.Domain;
using ClaimManagement.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Appication.Tests.Events
{
    public class ClaimAuditEventTests
    {
        [Test]
        public async Task Test_ClaimChangeEventNotification_ShouldPublishEventToEventGrid()
        {
            var configuration = TestHelper.GetConfiguration();
            var publishEvent = TestHelper.GetPublishEvent(configuration);

            ClaimChangeEventNotification claimChangeEvent = new ClaimChangeEventNotification()
            {
                ClaimAudit = new ClaimAudit()
                {
                    Id = 2,
                    Name = "Test Name",
                    Year = 2015,
                    DamageCost = 23.1M,
                    Type = ClaimType.Collision.GetDescription(),
                    CreatedAt = DateTime.UtcNow,
                    RequestName = "Add Claim"
                }
            };

            var handler = new ClaimChangeEventNotificationHandler(publishEvent);
            await handler.Handle(claimChangeEvent, CancellationToken.None);

            Assert.IsTrue(true);
        }
    }
}
