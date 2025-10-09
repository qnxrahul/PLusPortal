using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steer73.RockIT.Domain.Tests
{
    public static class TestData
    {
        public static Guid JobApplicationId = Guid.Parse("7a197a58-fcaf-445a-b25e-3aae9fbcd225");
        public static Guid JobFormDefinition1Id = Guid.Parse("9a197a58-fcaf-415a-b29e-3aae9fbad225");
        public static Guid DiversityFormDefinition1Id = Guid.Parse("52998bac-d220-435a-af43-e9cfbf26b395");
        public static Guid Vacancy1Id = Guid.Parse("73f6b156-8ac7-440a-b301-136094a341dc");
        public static Guid Vacancy2Id = Guid.Parse("1ea53cb4-5377-48ac-b281-cbd5bae6159b");

        public static Guid CompanyId = Guid.Parse("138c7a8f-3cad-46ba-a041-eece7286ce9b");
        public static Guid UserId = Guid.Parse("6f502362-3e73-48b3-8492-58d6ba6641bb");
        public static Guid PracticeGroupId = Guid.Parse("630a51a4-08e6-4d05-be55-6f99562255fc");

        public static string TestJobFormStructure = "TEST_JOB_FORM_STRUCTURE";
        public static string TestDiversityFormStructure = "TEST_DIVERSITY_FORM_STRUCTURE";
    }
}
