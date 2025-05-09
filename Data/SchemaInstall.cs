using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace i7MEDIA.Plugin.Widgets.Registry.Data;

[NopMigration("2025/05/07 00:00:00", "Widgets.Registry base schema", MigrationProcessType.Installation)]
public class SchemaInstall : Migration
{
    public override void Up()
    {
        var registry = NameCompatibilityManager.GetTableName(typeof(GiftRegistry));
        var registryItem = NameCompatibilityManager.GetTableName(typeof(GiftRegistryItem));

        if (!Schema.Table(registry).Exists())
        {
            Create.TableFor<GiftRegistry>();
        }

        if (!Schema.Table(registryItem).Exists())
        {
            Create.TableFor<GiftRegistryItem>();
        }
    }
    public override void Down() { }
}