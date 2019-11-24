using ClaimManagement.Domain.Entities;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.Application
{
    public interface IPublishEvent
    {
        Task PublishClaimAuditEvent(ClaimAudit claimAudit);
    }
    public class PublishEvent : IPublishEvent
    {
        private readonly IConfiguration _configuration;
        public PublishEvent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task PublishClaimAuditEvent(ClaimAudit claimAudit)
        {
            var events = new List<EventGridEvent>()
            {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = claimAudit.RequestName,
                    Data = JsonConvert.SerializeObject(claimAudit),
                    EventTime = DateTime.UtcNow,
                    Subject = claimAudit.Name,
                    DataVersion = "2.0"
                }
            };
            var eventGridTopicHostUri = _configuration["eventGridTopicHostUri"];
            var topicHostname = new Uri(@eventGridTopicHostUri).Host;
            var eventGridTopicCredentials = _configuration["eventGridTopicCredentials"];
            var topicCredentials = new TopicCredentials(eventGridTopicCredentials);
            var client = new EventGridClient(topicCredentials);

            await client.PublishEventsAsync(topicHostname, events);
        }
    }
}
