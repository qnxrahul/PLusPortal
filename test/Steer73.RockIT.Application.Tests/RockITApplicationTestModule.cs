using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Steer73.RockIT;

[DependsOn(
    typeof(RockITApplicationModule),
    typeof(RockITDomainTestModule)
)]
public class RockITApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
    }
}
