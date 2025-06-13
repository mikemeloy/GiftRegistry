using Nop.Core.Domain.Common;
using Nop.Core;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryType : BaseEntity, ISoftDeletedEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Deleted { get; set; }
}