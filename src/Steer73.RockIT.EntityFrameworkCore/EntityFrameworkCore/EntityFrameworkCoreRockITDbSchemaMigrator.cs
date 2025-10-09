using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Steer73.RockIT.Data;
using Volo.Abp.DependencyInjection;

namespace Steer73.RockIT.EntityFrameworkCore;

public class EntityFrameworkCoreRockITDbSchemaMigrator
    : IRockITDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreRockITDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the RockITDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<RockITDbContext>()
            .Database
            .MigrateAsync();
    }
}
