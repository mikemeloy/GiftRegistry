using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

[NopMigration("2025/05/07 00:00:00", "Widgets.Registry base schema", MigrationProcessType.Installation)]
public class SchemaInstall : Migration
{


    private readonly string _registry = NameCompatibilityManager.GetTableName(typeof(GiftRegistry));
    private readonly string _registryItem = NameCompatibilityManager.GetTableName(typeof(GiftRegistryItem));
    private readonly string _registryType = NameCompatibilityManager.GetTableName(typeof(GiftRegistryType));
    private readonly string _registryItemOrder = NameCompatibilityManager.GetTableName(typeof(GiftRegistryItemOrder));
    private readonly string _registryConsultant = NameCompatibilityManager.GetTableName(typeof(GiftRegistryConsultant));
    private readonly string _registryShipping = NameCompatibilityManager.GetTableName(typeof(GiftRegistryShippingOption));

    public override void Up()
    {
        if (!Schema.Table(_registry).Exists())
        {
            Create.TableFor<GiftRegistry>();
        }

        if (!Schema.Table(_registryItem).Exists())
        {
            Create.TableFor<GiftRegistryItem>();
        }

        if (!Schema.Table(_registryType).Exists())
        {
            Create.TableFor<GiftRegistryType>();
        }

        if (!Schema.Table(_registryItemOrder).Exists())
        {
            Create.TableFor<GiftRegistryItemOrder>();
        }

        if (!Schema.Table(_registryConsultant).Exists())
        {
            Create.TableFor<GiftRegistryConsultant>();
        }

        if (!Schema.Table(_registryShipping).Exists())
        {
            Create.TableFor<GiftRegistryShippingOption>();
        }
    }
    public override void Down()
    {
#if DEBUG
        if (Schema.Table(_registry).Exists())
        {
            Delete.Table(_registry);
        }

        if (Schema.Table(_registryItem).Exists())
        {
            Delete.Table(_registryItem);
        }

        if (Schema.Table(_registryType).Exists())
        {
            Delete.Table(_registryType);
        }

        if (Schema.Table(_registryItemOrder).Exists())
        {
            Delete.Table(_registryItemOrder);
        }

        if (Schema.Table(_registryConsultant).Exists())
        {
            Delete.Table(_registryConsultant);
        }

        if (Schema.Table(_registryShipping).Exists())
        {
            Delete.Table(_registryShipping);
        }
#endif
    }
}