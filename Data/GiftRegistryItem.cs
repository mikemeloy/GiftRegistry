using System;
using Nop.Core;
using Nop.Core.Domain.Common;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

public class GiftRegistryItem : BaseEntity, ISoftDeletedEntity
{
    public int RegistryId { get; set; }
    public int ProductId { get; set; }
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
    public int ShippingOption { get; set; }
    public string AttributesXml { get; set; }
    public bool Deleted { get; set; }
    public DateTime CreatedDate { get; set; }
}