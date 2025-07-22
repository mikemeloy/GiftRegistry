using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core;
using Nop.Core.Domain.Stores;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class PdfService : IRegistryPdfService
{
    private readonly IRegistryService _registry;
    private readonly IStoreContext _storeContext;

    private Store Store { get; set; }

    public PdfService(IRegistryService registry, IStoreContext storeContext)
    {
        _registry = registry;
        _storeContext = storeContext;
        Store = _storeContext.GetCurrentStore();
    }

    public async Task<byte[]> GenerateGiftReceiptAsync(int id)
    {
        var orderItems = await _registry.GetGiftReceiptOrderItemsAsync(id);

        var receipt = Document.Create(document =>
           {
               document.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(1, Unit.Centimetre);
                   page.PageColor(Colors.White);
                   page.DefaultTextStyle(x => x.FontSize(16));

                   page.Header().Element(container => ComposeGiftReceiptHeader(container, Store));
                   page.Content().Element((container) => ComposeGiftReceiptContent(container, orderItems));
                   page.Footer().Element(ComposeGiftReceiptFooter);

                   page.Background()
                      .AlignTop()
                      .AlignLeft()
                      .Padding(10)
                      .Text($"Order#: {id}");

                   page.Foreground()
                      .AlignTop()
                      .ExtendHorizontal()
                      .AlignRight()
                      .Padding(10)
                      .Text(DateTime.Now.ToShortDateString());
               });
           });

        return receipt.GeneratePdf();
    }

    public async Task<byte[]> GenerateRegistryReportAsync(ReportRequestDTO request)
    {
        var data = await _registry.GetReportDataAsync(request.Name, request.StartDate, request.EndDate, request.Status);

        var doc = Document.Create(document =>
           {
               document.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(1, Unit.Centimetre);
                   page.PageColor(Colors.White);
                   page.DefaultTextStyle(x => x.FontSize(10));

                   page.Content().Element((IContainer container) => ComposeRegistryReportContent(container, data));
               });
           });

        return doc.GeneratePdf();
    }

    public async Task<byte[]> GenerateRegistryItemReport(int registryId)
    {
        var data = await _registry.GetRegistryItemsByIdAsync(registryId);

        var doc = Document.Create(document =>
               {
                   document.Page(page =>
                   {
                       page.Size(PageSizes.A4);
                       page.Margin(1, Unit.Centimetre);
                       page.PageColor(Colors.White);
                       page.DefaultTextStyle(x => x.FontSize(10));

                       page.Content().Element((IContainer container) => ComposeItemReportContent(container, data));
                   });
               });

        return doc.GeneratePdf();
    }

    public async Task<byte[]> GenerateRegistryOrderReport(int registryId)
    {
        var data = await _registry.GetRegistryOrdersByIdAsync(registryId);

        var doc = Document.Create(document =>
               {
                   document.Page(page =>
                   {
                       page.Size(PageSizes.A4);
                       page.Margin(1, Unit.Centimetre);
                       page.PageColor(Colors.White);
                       page.DefaultTextStyle(x => x.FontSize(10));

                       page.Content().Element(container => ComposeOrderReportContent(container, data));
                   });
               });

        return doc.GeneratePdf();
    }

    private static void ComposeItemReportContent(IContainer container, IEnumerable<RegistryItemViewModel> registryItems)
    {
        container.Layers(layer =>
                            {
                                layer.PrimaryLayer()
                               .PaddingTop(25)
                               .Table(table =>
                                     {
                                         table.ColumnsDefinition(column =>
                                         {
                                             column.RelativeColumn();
                                             column.RelativeColumn();
                                             column.RelativeColumn();
                                             column.RelativeColumn();
                                             column.RelativeColumn();
                                             column.RelativeColumn();
                                         });

                                         table.Header(header =>
                                         {
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).Text("Product Name");
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Price");
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Quantity");
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Purchased");
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("In Stock");
                                             header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Fulfilled");
                                         });

                                         foreach (var registryItem in registryItems)
                                         {
                                             table.Cell().BorderBottom(1).AlignCenter().Padding(6).Text(registryItem.Name);
                                             table.Cell().BorderBottom(1).Padding(6).Text(registryItem.Price.ToCurrency());
                                             table.Cell().BorderBottom(1).AlignCenter().Padding(6).Text(registryItem.Quantity);
                                             table.Cell().BorderBottom(1).AlignCenter().Padding(6).Text(registryItem.Purchased);
                                             table.Cell().BorderBottom(1).AlignCenter().Padding(6).Text(registryItem.StockQuantity);
                                             table.Cell().BorderBottom(1).AlignCenter().Padding(6).Text(registryItem.Fulfilled ? "Yes" : "No");
                                         }
                                     });
                            });
    }

    private static void ComposeOrderReportContent(IContainer container, IEnumerable<RegistryOrderViewModel> orders)
    {
        container.Layers(layer =>
        {
            layer.PrimaryLayer()
           .PaddingTop(25)
           .Table(table =>
                 {
                     table.ColumnsDefinition(column =>
                     {
                         column.RelativeColumn();
                         column.RelativeColumn();
                         column.RelativeColumn();
                         column.RelativeColumn();
                     });

                     table.Header(header =>
                     {
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Order Id");
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Customer");
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Order Total");
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Order Date");
                     });

                     foreach (var orderItem in orders)
                     {
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.OrderId);
                         table.Cell().Padding(6).Text(orderItem.FullName);
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.OrderTotal.ToCurrency());
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.OrderDate);
                     }
                 });

        });
    }

    private static void ComposeRegistryReportContent(IContainer container, IEnumerable<RegistryListItem> registries)
    {
        container.Layers(layer =>
                {
                    layer.PrimaryLayer()
                   .PaddingTop(25)
                   .Table(table =>
                         {
                             table.ColumnsDefinition(column =>
                             {
                                 column.RelativeColumn();
                                 column.RelativeColumn();
                                 column.RelativeColumn();
                                 column.RelativeColumn();
                             });

                             table.Header(header =>
                             {
                                 header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).Text("Name");
                                 header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).Text("Owner");
                                 header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Description");
                                 header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Event Date");
                             });

                             foreach (var orderItem in registries)
                             {
                                 table.Cell().Padding(6).Text(orderItem.Name);
                                 table.Cell().Padding(6).Text(orderItem.Owner);
                                 table.Cell().Padding(6).Text(orderItem.Description);
                                 table.Cell().Padding(6).Text(orderItem.EventDate.ToShortDateString());
                             }
                         });

                });
    }

    private static void ComposeGiftReceiptHeader(IContainer container, Store store)
    {
        container.Column(column =>
        {
            column.Item()
                 .Table(table =>
                 {
                     table.ColumnsDefinition(column =>
                      {
                          column.RelativeColumn();
                      });

                     table.Cell().AlignCenter().Text(store.CompanyName).FontSize(24).FontColor("#621d20");
                     table.Cell().AlignCenter().Text(store.CompanyAddress).FontSize(10).ExtraLight();
                     table.Cell().AlignCenter().Text(store.CompanyPhoneNumber).FontSize(10).ExtraLight();
                 });
        });
    }

    private static void ComposeGiftReceiptContent(IContainer container, IEnumerable<GiftReceiptOrderItem> orderItems)
    {
        var size = PageSizes.Letter.Landscape();

        container.Layers(layer =>
        {
            layer.Layer()
             .AlignMiddle()
             .AlignCenter()
             .Text("Gift Receipt")
             .FontSize(75)
             .FontColor("#d1d1d1");

            layer.PrimaryLayer()
           .PaddingTop(50)
           .Table(table =>
                 {
                     table.ColumnsDefinition(column =>
                     {
                         column.RelativeColumn();
                         column.ConstantColumn(150);
                         column.ConstantColumn(100);
                     });

                     table.Header(header =>
                     {
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).Text("Description");
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("SKU");
                         header.Cell().BorderBottom(2).BorderColor("#621520").Padding(8).AlignCenter().Text("Quantity");
                     });

                     foreach (var orderItem in orderItems)
                     {
                         table.Cell().Padding(6).Text(orderItem.Description);
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.Sku);
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.Quantity);
                     }
                 });

        });
    }

    private static void ComposeGiftReceiptFooter(IContainer container)
    {
        container
            .AlignCenter().Text(x =>
                {
                    x.Span("Thank You For Your Purchase");
                });
    }
}