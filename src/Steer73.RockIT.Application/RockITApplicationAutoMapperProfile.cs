using AutoMapper;
using Steer73.RockIT.Companies;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.Enums;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.JobFormResponses;
using Steer73.RockIT.MediaSources;
using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.RoleTypes;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Vacancies;
using System;
using System.Linq;
using Volo.Abp.Identity;

namespace Steer73.RockIT;

public class RockITApplicationAutoMapperProfile : Profile
{
    public RockITApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Company, CompanyDto>();
        CreateMap<Company, CompanyExcelDto>();

        CreateMap<PracticeGroup, PracticeGroupDto>();

        CreateMap<PracticeArea, PracticeAreaDto>();
		//.ForMember(x => x.PracticeGroupName, opt => opt.MapFrom(src => src.Prac));

		CreateMap<Vacancy, VacancyDto>()		
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.GetStatus(DateTime.UtcNow) == Enums.VacancyStatus.Active || src.GetStatus(DateTime.UtcNow) == Enums.VacancyStatus.Closed))
		    .ForMember(dest => dest.VacancyStatus, opt => opt.MapFrom(src => src.GetStatus(DateTime.UtcNow).ToString()));

        CreateMap<VacancyWithNavigationProperties, VacancyWithNavigationPropertiesDto>();
        CreateMap<Company, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        CreateMap<IdentityUser, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

        CreateMap<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>();
        CreateMap<PracticeGroup, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Vacancy, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Title));

        CreateMap<FormDefinition, FormDefinitionDto>();
        CreateMap<FormDefinition, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));
        CreateMap<FormDefinition, FormDefinitionLookup<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

        CreateMap<JobApplication, JobApplicationDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.StatusAsString, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<JobApplicationWithNavigationProperties, JobApplicationWithNavigationPropertiesDto>();

        CreateMap<DiversityData, DiversityDataDto>();

        CreateMap<JobFormResponse, JobFormResponseDto>();

        CreateMap<DiversityFormResponse, DiversityFormResponseDto>();

        CreateMap<NewJobApplicationCompleteDto, NewJobApplicationComplete>();

        CreateMap<MediaSource, MediaSourceDto>();
        CreateMap<VacancyMediaSource, VacancyMediaSourceDto>();

        CreateMap<RoleType, RoleTypeDto>();
        CreateMap<VacancyRoleType, VacancyRoleTypeDto>();
        CreateMap<FormDefinitionWithNavigationProperties, FormDefinitionWithNavigationPropertiesDto>();

        CreateMap<VacancyContributor, VacancyContributorDto>()
            .ForMember(dest => dest.Contributor, opt => opt.MapFrom(src => src.IdentityUser)); ;
        CreateMap<IdentityUser, ContributorDto>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

        CreateMap<EzekiaCRM.default8, EzekiaProjectDto>();
    }
}