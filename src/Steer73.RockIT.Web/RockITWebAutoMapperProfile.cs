using AutoMapper;
using Steer73.RockIT.BrochureSubscriptions;
using Steer73.RockIT.Companies;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.JobFormResponses;
using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Web.Controllers;
// using Steer73.RockIT.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo; // removed for no-auth
using Steer73.RockIT.Web.Pages.Companies;
using Steer73.RockIT.Web.Pages.DiversityDatas;
using Steer73.RockIT.Web.Pages.DiversityFormResponses;
using Steer73.RockIT.Web.Pages.FormDefinitions;
using Steer73.RockIT.Web.Pages.JobApplications;
using Steer73.RockIT.Web.Pages.JobFormResponses;
using Steer73.RockIT.Web.Pages.PracticeAreas;
using Steer73.RockIT.Web.Pages.PracticeGroups;
using Steer73.RockIT.Web.Pages.Vacancies;
using System;
// using Volo.Abp.Account; // removed for no-auth
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using static Steer73.RockIT.Web.Pages.VacancyMoreInfoModel;

namespace Steer73.RockIT.Web;

public class RockITWebAutoMapperProfile : Profile
{
    public RockITWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<CompanyDto, CompanyUpdateViewModel>();
        CreateMap<CompanyUpdateViewModel, CompanyUpdateDto>();
        CreateMap<CompanyCreateViewModel, CompanyCreateDto>();

        CreateMap<PracticeGroupDto, PracticeGroupUpdateViewModel>();
        CreateMap<PracticeGroupUpdateViewModel, PracticeGroupUpdateDto>();
        CreateMap<PracticeGroupCreateViewModel, PracticeGroupCreateDto>();

        CreateMap<PracticeAreaDto, PracticeAreaUpdateViewModel>();
        CreateMap<PracticeAreaUpdateViewModel, PracticeAreaUpdateDto>();
        CreateMap<PracticeAreaCreateViewModel, PracticeAreaCreateDto>();

        // Map DateOnly to DateTime for ABP date picker. ABP date picker doesn't populate assigned date when DateOnly is used.
        CreateMap<VacancyDto, VacancyUpdateInputModel>()
            .ForMember(dest => dest.ClosingDate, opt => opt.MapFrom(src => src.ClosingDate.ToDateTime(new TimeOnly())))
            .ForMember(dest => dest.ExternalPostingDate, opt => opt.MapFrom(src => src.ExternalPostingDate.ToDateTime(new TimeOnly())))
            .ForMember(dest => dest.ExpiringDate, opt => opt.MapFrom(src => src.ExpiringDate.ToDateTime(new TimeOnly())));
       
        CreateMap<VacancyUpdateInputModel, VacancyUpdateDto>() 
            .ForMember(dest => dest.ClosingDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ClosingDate)))
            .ForMember(dest => dest.ExternalPostingDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ExternalPostingDate)))
            .ForMember(dest => dest.ExpiringDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ExpiringDate)));
        
        CreateMap<VacancyCreateInputModel, VacancyCreateDto>()
            .ForMember(dest => dest.ClosingDate, opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.ClosingDate!)))
            .ForMember(dest => dest.ExternalPostingDate, opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.ExternalPostingDate!)))
            .ForMember(dest => dest.ExpiringDate, opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.ExpiringDate!)));
         
        CreateMap<FormDefinitionDto, FormDefinitionUpdateViewModel>();
        CreateMap<FormDefinitionUpdateViewModel, FormDefinitionUpdateDto>();
        CreateMap<FormDefinitionCreateViewModel, FormDefinitionCreateDto>();

        CreateMap<JobApplicationDto, JobApplicationUpdateViewModel>();
        CreateMap<JobApplicationUpdateViewModel, JobApplicationUpdateDto>();
        CreateMap<JobApplicationCreateViewModel, JobApplicationCreateDto>();

        CreateMap<DiversityDataDto, DiversityDataUpdateViewModel>();
        CreateMap<DiversityDataUpdateViewModel, DiversityDataUpdateDto>();
        CreateMap<DiversityDataCreateViewModel, DiversityDataCreateDto>();

        CreateMap<JobFormResponseDto, JobFormResponseUpdateViewModel>();
        CreateMap<JobFormResponseUpdateViewModel, JobFormResponseUpdateDto>();
        CreateMap<JobFormResponseCreateViewModel, JobFormResponseCreateDto>();

        CreateMap<DiversityFormResponseDto, DiversityFormResponseUpdateViewModel>();
        CreateMap<DiversityFormResponseUpdateViewModel, DiversityFormResponseUpdateDto>();
        CreateMap<DiversityFormResponseCreateViewModel, DiversityFormResponseCreateDto>();

		CreateMap<JobApplicationWithNavigationPropertiesDto, JobApplicationViewModel>();


		CreateMap<BrochureSubscription, BrochureSubscriptionDto>();
		CreateMap<BrochureSubscriptionViewModel, BrochureSubscriptionCreateDto>();

        //CreateMap<Pages.Identity.Users.CustomCreateModalModel.CustomUserInfoViewModel, IdentityUserCreateDto>()
        //    .MapExtraProperties();

        //CreateMap<IdentityUserDto, Pages.Identity.Users.CustomEditModalModel.CustomUserInfoViewModel>()
        //    .MapExtraProperties();

        //CreateMap<Pages.Identity.Users.CustomEditModalModel.CustomUserInfoViewModel, IdentityUserUpdateDto>()
        //    .MapExtraProperties();

        // removed account profile mappings for no-auth

        CreateMap<JobApplicationModel, NewJobApplicationCompleteDto>()
            .Ignore(x => x.FileCv)
            .Ignore(x => x.FileCoverLetter)
            .Ignore(x => x.FileAdditionalDoc);
    }
}