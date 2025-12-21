using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.LanguageManagement.LanguageTexts;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Data;

public class LocalizationCleanupDataSeedContributor :
    IDataSeedContributor,
    ITransientDependency
{
    private const string ResourceName = "RockIT";
    private const string CultureName = "en";
    private const string LocalizationKey = "ShowDiversity";
    private const string DesiredValue = "Tick this box to use PL UK standard Diversity Form";

    private readonly LanguageTextManager _languageTextManager;

    public LocalizationCleanupDataSeedContributor(LanguageTextManager languageTextManager)
    {
        _languageTextManager = languageTextManager;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        await _languageTextManager.SetAsync(
            ResourceName,
            CultureName,
            LocalizationKey,
            DesiredValue,
            context.TenantId);
    }
}
