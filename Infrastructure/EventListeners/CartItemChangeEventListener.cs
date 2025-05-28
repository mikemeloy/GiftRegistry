using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Events;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure.EventListeners;

public class CartItemChangeEventListener : IConsumer<EntityDeletedEvent<ShoppingCartItem>>, IConsumer<OrderPlacedEvent>
{
    private readonly IRegistryService _registryService;
    public CartItemChangeEventListener(IRegistryService registryService)
    {
        _registryService = registryService;
    }

    public Task HandleEventAsync(EntityDeletedEvent<ShoppingCartItem> eventMessage)
    {
        var cartItem = eventMessage.Entity;

        return Task.CompletedTask;
    }

    public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
    {
        await _registryService.ReconcileRegistry(eventMessage.Order);
    }
}