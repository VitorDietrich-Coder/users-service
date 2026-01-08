using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Users.Microservice.Domain.Events;

namespace Users.Microservice.Infrastructure.Messaging
{
    public class SnsEventPublisher
    {
        private readonly IAmazonSimpleNotificationService _sns;
        private readonly string _topicArn;

        public SnsEventPublisher(
            IAmazonSimpleNotificationService sns,
            IConfiguration config)
        {
            _sns = sns;
            _topicArn = config["AWS:SNS:UserCreatedTopicArn"];
        }

        public async Task PublishUserCreatedAsync(UserCreatedEvent evt)
        {
            var message = JsonSerializer.Serialize(evt);

            await _sns.PublishAsync(new PublishRequest
            {
                TopicArn = _topicArn,
                Message = message
            });
        }
    }
}
