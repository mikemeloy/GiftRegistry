using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public IEnumerable<GiftReceiptOrderItem> OrderItems { get; set; }
    public Store Store { get; set; }

    public PdfService(IRegistryService registry, IStoreContext storeContext)
    {
        _registry = registry;
        _storeContext = storeContext;
    }

    public async Task<byte[]> GenerateGiftReceiptAsync(int id)
    {
        Store = _storeContext.GetCurrentStore();
        OrderItems = await _registry.GetGiftReceiptOrderItemsAsync(id);

        var receipt = Document.Create(document =>
           {
               document.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(1, Unit.Centimetre);
                   page.PageColor(Colors.White);
                   page.DefaultTextStyle(x => x.FontSize(16));

                   page.Header().Element(ComposeHeader);
                   page.Content().Element(ComposeContent);
                   page.Footer().Element(ComposeFooter);

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

    private void ComposeHeader(IContainer container)
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

                     table.Cell().AlignCenter().Text(Store.CompanyName).FontSize(24).FontColor("#621d20");
                     table.Cell().AlignCenter().Text(Store.CompanyAddress).FontSize(10).ExtraLight();
                     table.Cell().AlignCenter().Text(Store.CompanyPhoneNumber).FontSize(10).ExtraLight();
                 });
        });
    }

    private void ComposeContent(IContainer container)
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

                     foreach (var orderItem in OrderItems)
                     {
                         table.Cell().Padding(6).Text(orderItem.Description);
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.Sku);
                         table.Cell().AlignCenter().Padding(6).Text(orderItem.Quantity);
                     }
                 });

        });
    }

    private void ComposeFooter(IContainer container)
    {

        container
            .AlignCenter().Text(x =>
                {
                    x.Span("Thank You For Your Purchase");
                });
    }

    public async Task<byte[]> GenerateRegistryReportAsync(string name)
    {
        var data = await _registry.GetReportDataAsync();

        var receipt = Document.Create(document =>
           {
               document.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(1, Unit.Centimetre);
                   page.PageColor(Colors.White);
                   page.DefaultTextStyle(x => x.FontSize(16));

               });
           });

        return receipt.GeneratePdf();
    }
}