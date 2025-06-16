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
    }
    public override void Down()
    {
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
    }
}