using Nop.Core;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class RegistryType : BaseEntity
{
    public int StoreId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}