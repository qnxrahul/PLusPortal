using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum YesNoPreferNotToSay
{
    Yes = 1,
    No = 2,
    [Description("Prefer not to say")]
    PreferNotToSay = 3
}
