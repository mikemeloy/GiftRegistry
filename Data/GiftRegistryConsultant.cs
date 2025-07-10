using Nop.Core;
using Nop.Core.Domain.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryConsultant : BaseEntity, ISoftDeletedEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int SortOrder { get; set; }
    public bool Deleted { get; set; }
}