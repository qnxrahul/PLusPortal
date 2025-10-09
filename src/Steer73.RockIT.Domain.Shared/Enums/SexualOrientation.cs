using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum SexualOrientation
{
    Asexual = 1,
    Bisexual = 2,
    Heterosexual = 3,
    Gay = 4,
    Lesbian = 5,
    [Description("Prefer to self-describe")]
    PreferToSelfDescribe = 6,
    [Description("Prefer not to say")]
    PreferNotToSay = 7
}
