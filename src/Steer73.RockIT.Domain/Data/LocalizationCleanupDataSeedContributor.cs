using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.LanguageManagement;

namespace Steer73.RockIT.Data;

public class LocalizationCleanupDataSeedContributor :
    IDataSeedContributor,
    ITransientDependency
{
    private const string ResourceName = "RockIT";
    private const string CultureName = "en";
    private const string LocalizationKey = "ShowDiversity";
    private const string DesiredValue = "Tick this box to use PL UK standard Diversity Form";

    private readonly ILanguageTextRepository _languageTextRepository;

    public LocalizationCleanupDataSeedContributor(ILanguageTextRepository languageTextRepository)
    {
        _languageTextRepository = languageTextRepository;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        // LanguageManagement's repository returns tracked entities; updating the value inside a UoW is enough.
        var texts = await _languageTextRepository.GetListAsync(ResourceName, CultureName, cancellationToken: default);
        var text = texts?.Find(x => x.Name == LocalizationKey);
        if (text == null)
        {
            return;
        }

        if (text.Value == DesiredValue)
        {
            return;
        }

        text.Value = DesiredValue;
    }
}
