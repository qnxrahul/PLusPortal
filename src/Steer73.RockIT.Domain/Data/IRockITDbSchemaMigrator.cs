using System.Threading.Tasks;

namespace Steer73.RockIT.Data;

public interface IRockITDbSchemaMigrator
{
    Task MigrateAsync();
}
