using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.JobFormResponses;
using Steer73.RockIT.DiversityFormResponses;

using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Steer73.RockIT.JobApplications;

public class JobApplicationDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<JobApplication>>, ITransientDependency
{
    private readonly IDiversityDataRepository _diversityDataRepository;
    private readonly IJobFormResponseRepository _jobFormResponseRepository;
    private readonly IDiversityFormResponseRepository _diversityFormResponseRepository;

    public JobApplicationDeletedEventHandler(IDiversityDataRepository diversityDataRepository, IJobFormResponseRepository jobFormResponseRepository, IDiversityFormResponseRepository diversityFormResponseRepository)
    {
        _diversityDataRepository = diversityDataRepository;
        _jobFormResponseRepository = jobFormResponseRepository;
        _diversityFormResponseRepository = diversityFormResponseRepository;

    }

    public async Task HandleEventAsync(EntityDeletedEventData<JobApplication> eventData)
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
            await _diversityDataRepository.DeleteManyAsync(await _diversityDataRepository.GetListByJobApplicationIdAsync(eventData.Entity.Id));
            await _jobFormResponseRepository.DeleteManyAsync(await _jobFormResponseRepository.GetListByJobApplicationIdAsync(eventData.Entity.Id));
            await _diversityFormResponseRepository.DeleteManyAsync(await _diversityFormResponseRepository.GetListByJobApplicationIdAsync(eventData.Entity.Id));

        }
        catch
        {
            //...
        }
    }
}