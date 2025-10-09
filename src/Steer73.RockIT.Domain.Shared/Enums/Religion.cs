using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum Religion
{
    [Description("No Religion")] 
    NoReligion = 1,
    [Description("Christianity")]
    Christianity = 2,
    [Description("Buddhism")] 
    Buddhism = 3,
    [Description("Hinduism")] 
    Hinduism = 4,
    [Description("Judaism")]
    Judaism = 5,
    [Description("Islam")] 
    Islam = 6,
    [Description("Sikhism")] 
    Sikhism = 7,
    [Description("Prefer not to say")] 
    PreferNotToSay = 8,
    [Description("Other")] 
    Other = 9
}

