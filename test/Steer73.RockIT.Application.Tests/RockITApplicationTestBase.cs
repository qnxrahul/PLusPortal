using Volo.Abp.Modularity;

namespace Steer73.RockIT;

public abstract class RockITApplicationTestBase<TStartupModule> : RockITTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
