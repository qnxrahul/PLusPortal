using System;

namespace Steer73.RockIT.Web.Utilities
{
    public interface ILinkProvider
    {
        string GetLinkForVacancyDetailsPage(Guid vacancyId);
    }
}
