using Nop.Core;
using Nop.Core.Domain.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryShippingOption : BaseEntity, ISoftDeletedEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Deleted { get; set; }
}