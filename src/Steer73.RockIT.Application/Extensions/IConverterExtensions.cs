using DinkToPdf;
using DinkToPdf.Contracts;

namespace Steer73.RockIT.Extensions;

static class IConverterExtensions
{
    public static byte[] Convert(this IConverter converter, string html)
    {
        var htmlToPdfDocument = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                ColorMode = ColorMode.Color,    
                Orientation = Orientation.Landscape,    
                PaperSize = PaperKind.A4Plus
            },
            Objects =    
            {
                new ObjectSettings()    
                {

                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    HtmlContent = html,
                    PagesCount = true,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };

        return converter.Convert(htmlToPdfDocument);
    }
}
