using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum YesNoPreferNotToSayDontKnow
{
    Yes = 1,
    No = 2,
    [Description("Prefer not to say")]
    PreferNotToSay = 3,
    [Description("I don't know")]
    DontKnow = 4
}
