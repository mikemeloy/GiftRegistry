using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace i7MEDIA.Plugin.Widgets.Registry.Infrastructure;

internal class RouteProvider : IRouteProvider
{
    public int Priority => 0;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(
            name: RegistryDefaults.Index,
            pattern: "Registry/",
            defaults: new { controller = "Registry", action = "Index" }
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

        //Admin Section
        endpointRouteBuilder.MapControllerRoute(
           name: RegistryDefaults.Report,
           pattern: "admin/Registry/Report",
           defaults: new { controller = "Registry", action = "Report" }
        );
    }
}