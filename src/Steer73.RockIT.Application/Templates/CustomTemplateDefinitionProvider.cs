using Steer73.RockIT.Localization;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Razor;

namespace Steer73.RockIT.Templates;

public class CustomTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(new TemplateDefinition(
            TemplateNames.PdfLayout,
            typeof(RockITResource),
            defaultCultureName: "en",
            isLayout: true)
            .WithRazorEngine()
            .WithVirtualFilePath("/Templates/Pdf/Layout.cshtml", isInlineLocalized: true));

        context.Add(new TemplateDefinition(
            TemplateNames.JobResponsePdf,
            typeof(RockITResource),
            defaultCultureName: "en",
            layout: TemplateNames.PdfLayout)
            .WithRazorEngine()
            .WithVirtualFilePath("/Templates/Pdf/JobResponse.cshtml", isInlineLocalized: true));
    }
}
