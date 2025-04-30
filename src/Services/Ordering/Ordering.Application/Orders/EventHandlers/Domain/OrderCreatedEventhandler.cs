using MassTransit;
using Microsoft.FeatureManagement;
using Ordering.Application.Extentions;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventhandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventhandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {

        logger.LogInformation($"Domain Event Handle {domainEvent.GetType}");


        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var createedIntegrationEvent = domainEvent.Order.ToOrderDto();
            await publishEndpoint.Publish(createedIntegrationEvent, cancellationToken);
        }



    }
}
