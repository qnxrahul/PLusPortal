using Steer73.RockIT.MediaSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Data
{
    public class MediaSourcesDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        readonly IGuidGenerator _guidGenerator;
        readonly IRepository<MediaSource, Guid> _mediaSourceRepository;

        public MediaSourcesDataSeedContributor(
            IGuidGenerator guidGenerator,
            IRepository<MediaSource, Guid> mediaSourceRepository)
        {
            _guidGenerator = guidGenerator;
            _mediaSourceRepository = mediaSourceRepository;
        }


        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var existingMediaSources = (await _mediaSourceRepository.ToListAsync())
                .ToDictionary(m => m.Id);

            await CreateOrUpdateMediaSource(
                Guid.Parse("4ebaa4f5-9f0d-4304-b1fe-6ec9cea9df99"),
                "AFR",
                "AFR",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("fe513108-9dec-4d17-a6e8-051bdba1bb2e"),
                "The Australian",
                "The Australian",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("d8ad3471-068f-4392-ae3b-1db26498e26a"),
                "Association of Colleges for Teacher Education",
                "Association of Colleges for Teacher Education", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("83d8924b-7bf2-4807-991e-85a0a16cb087"),
                "American Education Research Association",
                "American Education Research Association", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("05de0cce-8a39-441c-842c-f1773905130b"),
                "The conversation for HE",
                "The conversation for HE",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("086cb71f-84c7-44fb-a67f-8ad99a4d6c51"),
                "Ethical jobs",
                "Ethical jobs", existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("a46a7b5f-2977-486b-a6dc-ba731455b857"),
                "Pro Bono Australia",
                "Pro Bono Australia",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("f38f5c47-89ff-4645-94e5-8687f296b652"),
                "Third Sector",
                "Third Sector",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("492a6e93-5935-438a-8309-76a5645c6c25"),
                "ACFID",
                "ACFID",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("7f76a03b-96c0-47ca-b502-196b2c26cdde"),
                "Times Educational Supplement",
                "Times Educational Supplement", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("8aa3729c-fb2f-495b-a727-2d36312eb5aa"),
                "AHISA",
                "AHISA",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("743d03d5-6da9-45e3-a8a8-8afccabfcb0a"),
                "IBO",
                "IBO", existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("a9fdc69a-3aa0-4e83-8e8b-e8fd7bebf919"),
                "Print - 中國教育報",
                "Print - 中國教育報", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("b1db8778-4e99-4e01-8f25-2463f2335a62"),
                "Print - 聯合報 United Daily",
                "Print - 聯合報 United Daily",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("9410d847-44a1-4d61-8a49-b6081d463328"),
                "Print – South China Morning Post",
                "Print – South China Morning Post", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("e96f0be0-5f86-44a5-bfd8-fd7da5c26d70"),
                "Website – CPJobs.com",
                "Website – CPJobs.com", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("9beb3abc-02ef-4a77-bca3-4e188c9bb393"),
                "Website – JobsDB",
                "Website – JobsDB", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("c404a66d-f923-47fe-8de2-2e8b113e3a23"),
                "Website - The Chronicle of Higher Education",
                "Website - The Chronicle of Higher Education", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("d92cf53e-b219-4b13-b872-96b928fac3f7"),
                "Website - HigherEdJobs",
                "Website - HigherEdJobs", existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("6095b040-99f3-447e-a9df-224521491415"),
                "University Affairs",
                "University Affairs", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2eda9c6c-9b50-4f29-992c-709bc63aa317"),
                "AcademicaCareers.com",
                "AcademicaCareers.com", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2ab7a2c0-fd8d-4545-84d0-7d5b59af2836"),
                "Inside Philanthropy",
                "Inside Philanthropy", existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("4aa356e4-d985-4d78-9c76-c3de2232941f"),
                "TimesHigherEd",
                "TimesHigherEd", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("7fc8ad34-ceac-463b-b81e-4bc2d6246f22"),
                "InsideHigherEd",
                "InsideHigherEd", existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2649f64c-dc97-4262-9fb8-dcc91441d75a"),
                "HigherEdJobs",
                "HigherEdJobs", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("cf2bd90d-7e1a-414c-b774-e54d2c3ea9a8"),
                "EnvironmentJobs.com",
                "EnvironmentJobs.com", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("229a5fe6-994c-4f56-8e88-0d10aff8968c"),
                "SWAAC",
                "SWAAC", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("1426f88c-ea33-4a91-ac93-4e56f1cfeb9f"),
                "ReliefWeb",
                "ReliefWeb", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("05eb03f1-4596-49d7-b19c-abacb76ba182"),
                "Diversity Jobs",
                "Diversity Jobs", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("47709606-c7ed-47c8-a5f4-9adcc132170c"),
                "American Society of Association Executives",
                "American Society of Association Executives",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("1a37ef19-55f6-4584-a776-5c2b016e8292"),
                "Society for Human Resource Management (SHRM)",
                "Society for Human Resource Management (SHRM)", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("3d4c4d31-674b-4a07-8249-00cb893aaffa"),
                "Tech Jobs for Good",
                "Tech Jobs for Good", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("edca3bd6-0d38-43ad-9908-6d298133db02"),
                "Academic Diversity Search",
                "Academic Diversity Search", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2c4b9ba7-cb74-498b-9317-631593958006"),
                "Women in Higher Education",
                "Women in Higher Education", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("f6310a14-0442-40bc-8622-61eeaff9e821"),
                "Indigenous Professionals",
                "Indigenous Professionals", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2317a298-2c1c-4152-92b6-d706911fd617"),
                "The Council of Foundations",
                "The Council of Foundations", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("1e3756b5-4add-48d5-ab2e-714062f686ff"),
                "Conservation Job Board",
                "Conservation Job Board",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("de2bcb4d-f999-4d87-9dbe-95d8c195aea1"),
                "Women in Development",
                "Women in Development", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("aad051bc-b2f6-4613-8e57-65b8af010819"),
                "Finance Jobs",
                "Finance Jobs", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("2bfbc062-39be-46df-bfe4-d0f964975e8e"),
                "CAUT",
                "CAUT", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("a30d1fd4-3a43-49e8-8f6e-58f3e8aeebfa"),
                "Net Impact",
                "Net Impact", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("af828f84-5be1-4a41-8cab-cb0826d58ea7"),
                "The Journal of Blacks in Higher Education",
                "The Journal of Blacks in Higher Education",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("3350e868-6758-4082-ac58-5ad506e0241c"),
                "Irish Times - Print",
                "Irish Times - Print", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("8855cf7c-052b-4e51-8cd8-38dae0d43fab"),
                "RecruitIreland.com (Irish Times online)",
                "RecruitIreland.com (Irish Times online)", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("19726dd9-c401-4f3f-bcdf-223f82e682c8"),
                "DUZ Wissenschaftskarriere",
                "DUZ Wissenschaftskarriere",
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("b58d0898-9275-4e3d-87e2-0386745493ac"),
                "academics.de",
                "academics.de", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("37c47d1d-6e48-4110-ba77-13c78aef4e9a"),
                "StepStone",
                "StepStone", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("d21f296a-c875-4833-ad30-9525e97e950c"),
                "https://www.lnvh.nl/",
                "https://www.lnvh.nl/", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("ea91ee6d-9e54-4b31-a396-ff17e4f69940"),
                "universityvacancies.com",
                "universityvacancies.com", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("dbe0cb0f-5947-47e8-8157-ff22f423748f"),
                "CASE",
                "CASE", 
                existingMediaSources);

            await CreateOrUpdateMediaSource(
                Guid.Parse("7e68b3f5-657b-4be0-a4c3-c74b250fa4b9"),
                "NIJobs.com",
                "NIJobs.com", 
                existingMediaSources);
        }

        private async Task<MediaSource> CreateOrUpdateMediaSource(
            Guid id,
            string name,
            string description,
            Dictionary<Guid, MediaSource> existingMediaSources)
        {
            existingMediaSources.TryGetValue(id, out MediaSource? mediaSource);
            if (mediaSource is null)
            {
                mediaSource = await _mediaSourceRepository.InsertAsync(new MediaSource(
                   id,
                   name,
                   description));
            }
            else
            {
                mediaSource.Name = name;
                mediaSource.Description = description;
            }
           
            return mediaSource;
        }
    }
}
