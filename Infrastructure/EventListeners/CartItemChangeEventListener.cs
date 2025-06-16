using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure.EventListeners;

public class CartItemChangeEventListener : IConsumer<OrderPlacedEvent>
{
    private readonly IRegistryService _registryService;
    public CartItemChangeEventListener(IRegistryService registryService)
    {
        _registryService = registryService;
    }

    public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
    {
        await _registryService.ReconcileRegistry(eventMessage.Order);
    }
}