using System;
using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryEditAdminModel(
                    int Id,
                    int DeliveryMethodId,
                    int EventTypeId,
                    int ConsultantId,
                    string AdminNotes,
                    string ClientNotes,
                    string Name,
                    string Summary,
                    DateTime EventDate,
                    string Sponsor,
                    List<RegistryItemViewModel> RegistryItems
                );