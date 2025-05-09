using System;
using Nop.Core;
using Nop.Core.Domain.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistry : BaseEntity, ISoftDeletedEntity
{
    public int StoreId { get; set; }
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Deleted { get; set; }
}