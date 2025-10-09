
using System.ComponentModel;

namespace Steer73.RockIT.Enums
{
    public enum VacancyStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Active")]
        Active = 2,
        [Description("Closed")]
        Closed = 3,

        [Description("Expired")]
        Expired = 4
    }
}
