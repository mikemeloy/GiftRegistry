using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Events;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure.EventListeners;

public class CartItemChangeEventListener : IConsumer<EntityInsertedEvent<ShoppingCartItem>>, IConsumer<EntityUpdatedEvent<ShoppingCartItem>>, IConsumer<EntityDeletedEvent<ShoppingCartItem>>, IConsumer<OrderPlacedEvent>
{
    public Task HandleEventAsync(EntityInsertedEvent<ShoppingCartItem> eventMessage)
    {
        var entity = eventMessage.Entity;

        return Task.CompletedTask;
    }

    public Task HandleEventAsync(EntityUpdatedEvent<ShoppingCartItem> eventMessage)
    {
        return Task.CompletedTask;
    }

    public Task HandleEventAsync(EntityDeletedEvent<ShoppingCartItem> eventMessage)
    {
        return Task.CompletedTask;
    }

    public Task HandleEventAsync(OrderPlacedEvent eventMessage)
    {
        return Task.CompletedTask;
    }
}