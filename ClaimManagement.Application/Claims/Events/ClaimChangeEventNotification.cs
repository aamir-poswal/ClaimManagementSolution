using ClaimManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimManagement.Application.Claims.Events
{
    public class ClaimChangeEventNotification : INotification
    {
        public ClaimAudit ClaimAudit { get; set; }
    }
    public class ClaimChangeEventNotificationHandler : INotificationHandler<ClaimChangeEventNotification>
    {
        private readonly IPublishEvent _publishEvent;

        public ClaimChangeEventNotificationHandler(IPublishEvent publishEvent)
        {
            _publishEvent = publishEvent;
        }

        public async Task Handle(ClaimChangeEventNotification notification, CancellationToken cancellationToken)
        {
            await _publishEvent.PublishClaimAuditEvent(notification.ClaimAudit);
        }
    }

}
