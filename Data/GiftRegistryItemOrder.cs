using Nop.Core;
using Nop.Core.Domain.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryItemOrder : BaseEntity, ISoftDeletedEntity
{
    public int OrderId { get; set; }
    public int RegistryItemId { get; set; }
    public int Quantity { get; set; }
    public bool External { get; set; }
    public string Notes { get; set; }
    public bool Deleted { get; set; }
}