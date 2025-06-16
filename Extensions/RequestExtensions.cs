using i7MEDIA.Plugin.Widgets.Registry.Models;

namespace i7MEDIA.Plugin.Widgets.Registry.Extensions;

public static class RequestExtensions
{
    public static bool IsValid(this RegistryAddProductRequest source)
    {
        if (source.IsNull() || source.GiftRegistryId.IsNull() || source.ProductId.IsNull() || source.Quantity < 1)
        {
            return false;
        }

        return true;
    }
}
