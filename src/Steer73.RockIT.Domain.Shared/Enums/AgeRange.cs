using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum AgeRange
{
    [Description("16-25")]
    Age16to25 = 1,
    [Description("26-35")]
    Age26to35 = 2,
    [Description("36-45")]
    Age36to45 = 3,
    [Description("46-55")]
    Age46to55 = 4,
    [Description("Over 55")]
    Over55 = 5,
    [Description("Prefer not to say")]
    PreferNotToSay = 6
}
