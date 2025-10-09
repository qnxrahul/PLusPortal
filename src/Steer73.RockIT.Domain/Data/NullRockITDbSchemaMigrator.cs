using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Steer73.RockIT.Data;

/* This is used if database provider does't define
 * IRockITDbSchemaMigrator implementation.
 */
public class NullRockITDbSchemaMigrator : IRockITDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
