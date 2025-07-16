using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure;

internal class RouteProvider : IRouteProvider
{
    public int Priority => 0;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        RegisterAdminRoutes(endpointRouteBuilder);
        RegisterPublicRoutes(endpointRouteBuilder);
    }
    private static void RegisterPublicRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
           name: RegistryDefaults.Index,
           pattern: "Registry/",
           defaults: new { controller = "Registry", action = "Index" }
        );
        endpointRouteBuilder.MapControllerRoute(
             name: RegistryDefaults.Get,
             pattern: "Registry/Get",
             defaults: new { controller = "Registry", action = "Get" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.List,
            pattern: "Registry/List",
            defaults: new { controller = "Registry", action = "List" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.Registry,
            pattern: "Registry/{id?}",
            defaults: new { controller = "Registry", action = "Registry" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.Insert,
            pattern: "Registry/Insert",
            defaults: new { controller = "Registry", action = "Insert" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.Update,
            pattern: "Registry/Update",
            defaults: new { controller = "Registry", action = "Update" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.Save,
            pattern: "Registry/Save",
            defaults: new { controller = "Registry", action = "Save" }
        );
        endpointRouteBuilder.MapControllerRoute(
              name: RegistryDefaults.DeleteItem,
              pattern: "Registry/Item/{id?}",
              defaults: new { controller = "Registry", action = "DeleteItem" }
        );
        endpointRouteBuilder.MapControllerRoute(
             name: RegistryDefaults.Delete,
             pattern: "Registry/{id?}",
             defaults: new { controller = "Registry", action = "Delete" }
        );
        endpointRouteBuilder.MapControllerRoute(
           name: RegistryDefaults.AddToCart,
           pattern: "Registry/AddToCart/{id?}",
           defaults: new { controller = "Registry", action = "AddToCart" }
        );
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.PrintGiftReceipt,
            pattern: "Registry/PrintGiftReceipt",
            defaults: new { controller = "Registry", action = "PrintGiftReceipt" }
        );
    }
    private static void RegisterAdminRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
           name: AdminDefaults.Index,
           pattern: "Admin/Index",
           defaults: new { controller = "Admin", action = "Index", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.Registry,
            pattern: "Admin/Registry",
            defaults: new { controller = "Admin", action = "Registry", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.Get,
            pattern: "Admin/Registry/Get",
            defaults: new { controller = "Admin", action = "Registry/Get" }
        );

        endpointRouteBuilder.MapControllerRoute(
           name: AdminDefaults.Consultant,
           pattern: "Admin/Consultant",
           defaults: new { controller = "Admin", action = "Consultant", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.RegistryType,
            pattern: "Admin/RegistryType",
            defaults: new { controller = "Admin", action = "RegistryType", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.RegistryShipping,
            pattern: "Admin/ShippingOption",
            defaults: new { controller = "Admin", action = "ShippingOption", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
           name: AdminDefaults.ConsultantList,
           pattern: "Admin/Consultant/List",
           defaults: new { controller = "Admin", action = "Consultant/List", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
           name: AdminDefaults.RegistryTypeList,
           pattern: "Admin/RegistryType/List",
           defaults: new { controller = "Admin", action = "RegistryType/List", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
           name: AdminDefaults.RegistryShippingList,
           pattern: "Admin/ShippingOption/List",
           defaults: new { controller = "Admin", action = "ShippingOption/List", area = AreaNames.Admin }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.RegistryItem,
            pattern: "Admin/Registry/Item/{id?}",
            defaults: new { controller = "Admin", action = "DeleteItem" }
        );

        endpointRouteBuilder.MapControllerRoute(
            name: AdminDefaults.RegistryAdminReport,
            pattern: "Admin/Report",
            defaults: new { controller = "Admin", action = "Report" }
        );
    }
}