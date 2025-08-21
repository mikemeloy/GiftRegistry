using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Utils;

public interface IUtils
{
    public Task ResetProductKeyValidAsync();
}

public class Utility : IUtils
{
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly IStoreContext _storeContext;
    public Utility(IGenericAttributeService genericAttributeService, IStoreContext storeContext)
    {
        _genericAttributeService = genericAttributeService;
        _storeContext = storeContext;
    }
    public async Task ResetProductKeyValidAsync()
    {
        var store = _storeContext.GetCurrentStore();
        await _genericAttributeService.SaveAttributeAsync<bool?>(store, RegistryDefaults.ProductKeyValidAttribute, null, store.Id);
    }
}
