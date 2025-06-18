using Nop.Core.Domain.Common;
using Nop.Core;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryConsultant : BaseEntity, ISoftDeletedEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Deleted { get; set; }
}