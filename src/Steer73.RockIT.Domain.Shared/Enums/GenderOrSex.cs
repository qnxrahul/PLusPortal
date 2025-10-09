using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum GenderOrSex
{
    [Description("Cisgender Man")]
    CisgenderMan = 1,
    [Description("Cisgender Woman")]
    CisgenderWoman = 2,
    [Description("Intersex")]
    Intersex = 3,
    [Description("Non-Binary")]
    NonBinary = 4,
    [Description("Prefer not to say")]
    PreferNotToSay = 5,
    [Description("Transgender Man")]
    TransgenderMan = 6,
    [Description("Transgender Woman")]
    TransgenderWoman = 7,
    [Description("Undisclosed")]
    Undisclosed = 8,
    [Description("Other")]
    Other = 9
}
