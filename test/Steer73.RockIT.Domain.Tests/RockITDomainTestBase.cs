using Volo.Abp.Modularity;

namespace Steer73.RockIT;

/* Inherit from this class for your domain layer tests. */
public abstract class RockITDomainTestBase<TStartupModule> : RockITTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
