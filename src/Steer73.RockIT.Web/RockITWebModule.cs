using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
// Remove external authentication providers for anonymous portal
using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Extensions.DependencyInjection; // removed
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Companies.External;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.Localization;
using Steer73.RockIT.MultiTenancy;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.Web.HealthChecks;
using Steer73.RockIT.Web.Menus;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Volo.Abp;
// Remove ABP Account packages for no-auth runtime
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Emailing;
using Volo.Abp.Gdpr.Web;
using Volo.Abp.Gdpr.Web.Extensions;
// using Volo.Abp.Identity;
// using Volo.Abp.Identity.Web;
using Volo.Abp.LanguageManagement;
// using Volo.Abp.LeptonX.Shared; // removed
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
// Remove OpenIddict Pro Web for OSS/no-auth
using Volo.Abp.PermissionManagement;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.Timing;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
// using Volo.Saas.Host; // removed for OSS

namespace Steer73.RockIT.Web;

[DependsOn(
    typeof(RockITHttpApiModule),
    typeof(RockITApplicationModule),
    typeof(RockITEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpAuditLoggingWebModule),
    typeof(LanguageManagementWebModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(TextTemplateManagementWebModule),
    typeof(AbpGdprWebModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpBackgroundJobsModule),
    typeof(AbpBackgroundWorkersModule)
)]
public class RockITWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RockITViewModelExtensions.Configure();
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(RockITResource),
                typeof(RockITDomainModule).Assembly,
                typeof(RockITDomainSharedModule).Assembly,
                typeof(RockITApplicationModule).Assembly,
                typeof(RockITApplicationContractsModule).Assembly,
                typeof(RockITWebModule).Assembly
            );
        });

        // Authentication removed for local debugging; all pages accessible anonymously

        // Removed OpenIddict server configuration for no-auth
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.AddApplicationInsightsTelemetry();

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        }

        // No OpenIddict server when authentication is removed

        // Authentication removed
        // Theme-specific bundling removed
        ConfigureUrls(configuration);
        ConfigurePages(configuration);
        // Impersonation removed (no auth)
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        // External providers removed
        ConfigureHealthChecks(context);
        ConfigureCookieConsent(context);
        // Theme-specific configuration removed

        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = true;
        });

        context.Services.AddOptions();
        context.Services.Configure<EzekiaConfiguration>(configuration.GetSection(nameof(EzekiaConfiguration)));
        context.Services.AddHttpClient<EzekiaCRM.IClient, EzekiaCRM.Client>(client =>
        {
            var apiKey = configuration.GetValue<string>("EzekiaConfiguration:ApiKey");
            var apiUrl = configuration.GetSection("EzekiaConfiguration:BaseApiUrl").Value;

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            client.Timeout = TimeSpan.FromSeconds(CompanyConsts.ExternalRequestTimeoutInSec);
        });
        context.Services.AddHttpContextAccessor();

        Configure<AbpAspNetCoreContentOptions>(options =>
        {
            options.AllowedExtraWebContentFolders.Add("/Areas");
        });

        if (Debugger.IsAttached)
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }

        Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
    }
    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv)
    {
        var fileName = "authserver.pfx";
        var passPhrase = "HBjx6M4XmNdm3vwF";
        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);

        if (!File.Exists(file))
        {
            throw new FileNotFoundException($"Signing Certificate couldn't found: {file}");
        }

        return new X509Certificate2(file, passPhrase);
    }

    private void ConfigureCookieConsent(ServiceConfigurationContext context) { }

    // Removed ConfigureTheme (LeptonX-specific)

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddRockITHealthChecks();
    }

    // Removed ConfigureBundles (LeptonX-specific)

    private void ConfigurePages(IConfiguration configuration)
    {
        Configure<RazorPagesOptions>(options =>
        {

            // Remove authorization: make all pages accessible
		});
    }

    // Authentication removed

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    // Authentication removed

    // Impersonation removed

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<RockITWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RockITWebModule>();

            if (hostingEnvironment.IsDevelopment())
            {
                options.FileSets.ReplaceEmbeddedByPhysical<RockITDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Steer73.RockIT.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<RockITDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Steer73.RockIT.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<RockITApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Steer73.RockIT.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<RockITApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}Steer73.RockIT.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<RockITHttpApiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Steer73.RockIT.HttpApi", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<RockITWebModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new RockITMenuContributor());
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new RockITToolbarContributor());
        });

        // Remove profile management page contributor (no account pages)
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(RockITApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RockIT API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    // External providers removed

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {

        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var config = context.GetConfiguration();

        var settingManager = context.ServiceProvider.GetService<SettingManager>();
        //encrypts the password on set and decrypts on get
        await settingManager.SetGlobalAsync(EmailSettingNames.Smtp.Password, config.GetValue<string>("Settings.Abp.Mailing.Smtp.Password"));
        
        await context.AddBackgroundWorkerAsync<CompanyUpdaterWorker>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        // Cookie consent removed
        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        // Authentication removed

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        // Authorization removed (no auth)
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "RockIT API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
