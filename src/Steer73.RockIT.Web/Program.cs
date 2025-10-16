using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Serilog;
using Serilog.Events;

namespace Steer73.RockIT.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        // Configure early logging for startup errors
        var logFilePath = Environment.GetEnvironmentVariable("HOME") != null 
            ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), "LogFiles", "startup-log.txt")
            : "startup-log.txt";
            
        try
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
                
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsSecretsJson()
#if DEBUG       
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration

                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File("Logs/logs.txt"))
                        .WriteTo.Async(c => c.Console());
                });
#else
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                        .WriteTo.Console();
                        
                    // Add Application Insights if configured
                    var appInsightsConnectionString = context.Configuration["ApplicationInsights:ConnectionString"];
                    if (!string.IsNullOrEmpty(appInsightsConnectionString))
                    {
                        try
                        {
                            loggerConfiguration.WriteTo.ApplicationInsights(
                                appInsightsConnectionString,
                                new Serilog.Sinks.ApplicationInsights.TelemetryConverters.TraceTelemetryConverter());
                        }
                        catch
                        {
                            // Application Insights configuration failed, continue without it
                        }
                    }
                });
#endif


            string? appConfigUrl = builder.Configuration["AppConfig:Url"];
            int appConfigExpirationMinutes = builder.Configuration.GetValue<int>("AppConfig:ExpirationMinutes");
            string buildConfiguration = builder.Configuration.GetValue<string>("AppConfig:BuildConfiguration");

            //if (!string.IsNullOrEmpty(appConfigUrl))
            //{
            //    builder.Configuration.AddAzureAppConfiguration(options =>
            //    {
            //        var credentials = new DefaultAzureCredential();

            //        options.Connect(
            //            new Uri(appConfigUrl),
            //            credentials)
            //            .Select(KeyFilter.Any, LabelFilter.Null)
            //            .Select(KeyFilter.Any, buildConfiguration)
            //            .ConfigureRefresh(refreshOptions =>
            //                refreshOptions.Register("RockITATS:Settings:Sentinel", refreshAll: true)
            //                    .SetCacheExpiration(TimeSpan.FromMinutes(appConfigExpirationMinutes))
            //            );

            //        options.ConfigureKeyVault(kv => kv.SetCredential(credentials));

            //    });
            //}

            await builder.AddApplicationAsync<RockITWebModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            
            // Write detailed error to file for Azure diagnostics
            var errorFilePath = Environment.GetEnvironmentVariable("HOME") != null 
                ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), "LogFiles", "startup-error.txt")
                : "startup-error.txt";
                
            try
            {
                var errorDetails = $"Startup Error at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC\n" +
                                 $"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Not Set"}\n" +
                                 $"Machine: {Environment.MachineName}\n" +
                                 $"OS: {Environment.OSVersion}\n" +
                                 $"Exception Type: {ex.GetType().FullName}\n" +
                                 $"Message: {ex.Message}\n" +
                                 $"Stack Trace:\n{ex.StackTrace}\n";
                
                if (ex.InnerException != null)
                {
                    errorDetails += $"\nInner Exception:\n" +
                                  $"Type: {ex.InnerException.GetType().FullName}\n" +
                                  $"Message: {ex.InnerException.Message}\n" +
                                  $"Stack Trace:\n{ex.InnerException.StackTrace}\n";
                }
                
                File.WriteAllText(errorFilePath, errorDetails);
            }
            catch
            {
                // If we can't write the file, at least the exception will be in the logs
            }
            
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
