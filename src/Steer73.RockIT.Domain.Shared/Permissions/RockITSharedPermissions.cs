using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steer73.RockIT.Permissions
{
    public static class RockITSharedPermissions
    {
        public const string GroupName = "RockIT";

        public static class Companies
        {
            public const string Default = GroupName + ".Companies";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class PracticeGroups
        {
            public const string Default = GroupName + ".PracticeGroups";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class PracticeAreas
        {
            public const string Default = GroupName + ".PracticeAreas";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class Vacancies
        {
            public const string Default = GroupName + ".Vacancies";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class FormDefinitions
        {
            public const string Default = GroupName + ".FormDefinitions";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
            public const string View = Default + ".View";
        }

        public static class VacancyFormDefinitions
        {
            public const string Default = GroupName + ".VacancyFormDefinitions";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class JobApplications
        {
            public const string Default = GroupName + ".JobApplications";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class DiversityDatas
        {
            public const string Default = GroupName + ".DiversityDatas";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class JobFormResponses
        {
            public const string Default = GroupName + ".JobFormResponses";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class DiversityFormResponses
        {
            public const string Default = GroupName + ".DiversityFormResponses";
            public const string Edit = Default + ".Edit";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }
    }
}
