using Steer73.RockIT.BrochureSubscriptions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;

namespace Steer73.RockIT
{
    public class EmailAppService : IEmailAppService, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        protected IBlobContainer<VacancyFileContainer> _blobContainer;
        protected IRepository<AppFileDescriptors.AppFileDescriptor, Guid> _appFileDescriptorRepository;
        private static readonly Guid RegistrationTemplateVacancyId = Guid.Parse("77560497-e1a3-23bd-6399-3a1c2ca9a1ff");
        private const string RegistrationTemplatePath = "EmailTemplates/RegistrationTemplate.txt";

        public EmailAppService(
            IEmailSender emailSender,
            IBlobContainer<VacancyFileContainer> blobContainer,
            IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository)
        {
            _emailSender = emailSender;
            _blobContainer = blobContainer;
            _appFileDescriptorRepository = appFileDescriptorRepository;
        }

        public async Task QueueBrochureEmail(
            string emailTemplatePath,
            string brochureLink,
            BrochureSubscriptionDto subscription,
            VacancyWithNavigationPropertiesDto vacancy)
        {
			const string subject = "Your brochure request has been received.";

            var fileDescriptor = await _appFileDescriptorRepository.GetAsync(vacancy.Vacancy.BrochureFileId.Value);

            string body = await System.IO.File.ReadAllTextAsync(emailTemplatePath);
            var fileBytes = await _blobContainer.GetAllBytesAsync(fileDescriptor.Id.ToString("N"));

            body = body.Replace("{FirstName}", subscription.FirstName)
                             .Replace("{LastName}", subscription.LastName)
                             .Replace("{CompanyName}", vacancy.Company.Name)
                             .Replace("{VacancyProjectId}", vacancy.Vacancy.ProjectId?.ToString())
                             .Replace("{GeneratedLink}", brochureLink);

            var attachments = new List<EmailAttachment>
            {
                new EmailAttachment { Name = fileDescriptor.Name, File = fileBytes }
            };

            if (vacancy.Vacancy.AdditionalFileId.HasValue)
            {
                var additionalFileDescriptor = await _appFileDescriptorRepository.GetAsync(vacancy.Vacancy.AdditionalFileId.Value);
                var additionalFileBytes = await _blobContainer.GetAllBytesAsync(additionalFileDescriptor.Id.ToString("N"));
                attachments.Add(new EmailAttachment { Name = additionalFileDescriptor.Name, File = additionalFileBytes });
            }

            await _emailSender.QueueAsync(
                from: vacancy.IdentityUser.Email,
                to: subscription.EmailAddress,
                subject: subject,
                body: body,
                isBodyHtml: false,
                additionalEmailSendingArgs: new AdditionalEmailSendingArgs
            {
                Attachments = attachments
            });
        }

        public async Task QueueNoBrochureEmail(
            string emailTemplatePath,
            string brochureLink,
            BrochureSubscriptionDto subscription,
            VacancyWithNavigationPropertiesDto vacancy)
        {
			const string subject = "Your brochure request has been received.";

			string body = await System.IO.File.ReadAllTextAsync(emailTemplatePath);

            body = body.Replace("{FirstName}", subscription.FirstName)
                             .Replace("{LastName}", subscription.LastName)
                             .Replace("{CompanyName}", vacancy.Company.Name)
                             .Replace("{VacancyProjectId}", vacancy.Vacancy.ProjectId?.ToString())
                             .Replace("{GeneratedLink}", brochureLink);

            await _emailSender.QueueAsync(
                from: vacancy.IdentityUser.Email,
                to: subscription.EmailAddress,
                subject: subject,
                body: body,
                isBodyHtml: false);
        }

		public async Task QueueNewJobApplicationEmail(
			string emailTemplatePath,
			JobApplicationDto jobApplication,
			VacancyWithNavigationPropertiesDto vacancy)
		{
			var isRegistrationVacancy = vacancy?.Vacancy?.Id == RegistrationTemplateVacancyId;
			var templatePath = isRegistrationVacancy ? RegistrationTemplatePath : emailTemplatePath;
			var subject = isRegistrationVacancy
				? "Thank you for registering with Perrett Laver"
				: "Your job application was received.";

			string body = await System.IO.File.ReadAllTextAsync(templatePath);
            var akaOrFirstName = string.IsNullOrWhiteSpace(jobApplication.Aka) 
                ? jobApplication.FirstName : jobApplication.Aka;

            body = body.Replace("{FirstName}", akaOrFirstName)			
                .Replace("{CompanyName}", vacancy.Company.Name)			
                .Replace("{VacancyProjectId}", vacancy.Vacancy?.ProjectId?.ToString() ?? string.Empty)		 
                .Replace("{VacancyTitle}", vacancy.Vacancy?.Title);

			await _emailSender.QueueAsync(
                from: vacancy.IdentityUser.Email,
			    to: jobApplication.EmailAddress,
			    subject: subject,
			    body: body,
				isBodyHtml: false);
		}
	}
}
