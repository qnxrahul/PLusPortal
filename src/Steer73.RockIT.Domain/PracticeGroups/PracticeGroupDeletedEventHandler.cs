using Steer73.RockIT.PracticeAreas;

using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Steer73.RockIT.PracticeGroups;

public class PracticeGroupDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<PracticeGroup>>, ITransientDependency
{
    private readonly IPracticeAreaRepository _practiceAreaRepository;

    public PracticeGroupDeletedEventHandler(IPracticeAreaRepository practiceAreaRepository)
    {
        _practiceAreaRepository = practiceAreaRepository;

    }

    public async Task HandleEventAsync(EntityDeletedEventData<PracticeGroup> eventData)
    {
        if (eventData.Entity is not ISoftDelete softDeletedEntity)
        {
            return;
        }

        if (!softDeletedEntity.IsDeleted)
        {
            return;
        }

        try
        {
            await _practiceAreaRepository.DeleteManyAsync(await _practiceAreaRepository.GetListByPracticeGroupIdAsync(eventData.Entity.Id));

        }
        catch
        {
            //...
        }
    }
}