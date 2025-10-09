using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using Volo.Abp.Account; // removed for OSS/no-auth
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Azure;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
// using Volo.Abp.OpenIddict; // removed for OSS/no-auth
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.VirtualFileSystem;
// using Volo.Saas.Host; // removed for OSS
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Steer73.RockIT.Templates;
namespace Steer73.RockIT;
[DependsOn(
typeof(RockITDomainModule),
    typeof(RockITApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    // Remove SaaS Host Application for OSS
    typeof(AbpAuditLoggingApplicationModule),
    // Remove Pro OpenIddict & Account modules for OSS/no-auth
    typeof(LanguageManagementApplicationModule),
    typeof(AbpGdprApplicationModule),
    typeof(TextTemplateManagementApplicationModule),
	typeof(AbpTextTemplatingRazorModule)
	)]
[DependsOn(typeof(AbpBlobStoringAzureModule))]
public class RockITApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<RockITApplicationModule>();
        });

        Configure<AbpBlobStoringOptions>(options => 
        {
            options.Containers.ConfigureDefault(container =>
            {
                var storageConnectionString = configuration.GetValue<string>("RockITATS:AzureBlobStorage:ConnectionString");

                if (!string.IsNullOrEmpty(storageConnectionString))
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = storageConnectionString;
                        azure.CreateContainerIfNotExists = true;
                    });
                }
            });
        });

		Configure<AbpRazorTemplateCSharpCompilerOptions>(options =>
		{
			options.References.Add(MetadataReference.CreateFromFile(typeof(RockITApplicationModule).Assembly.Location));
		});
		Configure<AbpVirtualFileSystemOptions>(options =>
		{
			options.FileSets.AddEmbedded<RockITApplicationModule>("Steer73.RockIT");
		});

        Configure<AbpCompiledViewProviderOptions>(options =>
        {
            options.TemplateReferences.Add(TemplateNames.JobResponsePdf, new List<Assembly>()
            {
                Assembly.Load("System.Text.Json")
            }   
            .Select(x => MetadataReference.CreateFromFile(x.Location))   
            .ToList());
        });

        context.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
    }
}
