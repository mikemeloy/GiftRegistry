using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class PdfService : IRegistryPdfService
{
    public async Task<byte[]> GenerateGiftReceiptAsync()
    {
        var doc = Document.Create(container =>
           {
               container.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(2, Unit.Centimetre);
                   page.PageColor(Colors.White);
                   page.DefaultTextStyle(x => x.FontSize(20));

                   page.Header()
                       .Text("Hello PDF!")
                       .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                   page.Content()
                       .PaddingVertical(1, Unit.Centimetre)
                       .Column(x =>
                       {
                           x.Spacing(20);

                           x.Item().Text(Placeholders.LoremIpsum());
                           x.Item().Image(Placeholders.Image(200, 100));
                       });

                   page.Footer()
                       .AlignCenter()
                       .Text(x =>
                       {
                           x.Span("Page ");
                           x.CurrentPageNumber();
                       });
               });
           });


        var x = doc.GeneratePdf();

        return x;
    }
}
