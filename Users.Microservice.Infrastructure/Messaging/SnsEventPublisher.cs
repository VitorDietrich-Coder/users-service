using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
 
using Users.Microservice.Infrastructure.Interfaces;

namespace Users.Microservice.Infrastructure.Messaging;

public class SnsEventBus : IEventBus
{
    private readonly IAmazonSimpleNotificationService _sns;
    private readonly string _topicArn;

    public SnsEventBus(
        IAmazonSimpleNotificationService sns,
        IConfiguration config)
    {
        _sns = sns;
        _topicArn = config["AWS:SNS:UserCreatedTopicArn"];
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string queueName)
        where TEvent : class
    {
        var message = JsonSerializer.Serialize(@event);

        await _sns.PublishAsync(new PublishRequest
        {
            TopicArn = _topicArn,
            Message = message,
            MessageAttributes =
            {
                {
                    "eventType",
                    new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(TEvent).Name
                    }
                }
            }
        });
    }
}
