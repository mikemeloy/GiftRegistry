using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryProductAttribute(int AttributeId, string AttributeValue, bool IsRequired, int ControlTypeId);

public record RegistryAddProductRequest(int ProductId, int GiftRegistryId, int Quantity, IEnumerable<RegistryProductAttribute> Attributes);