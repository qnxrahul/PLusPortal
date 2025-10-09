using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum EducationLevel
{
    [Description("Prefer not to say")] 
    PreferNotToSay = 1,
    [Description("Upper secondary")] 
    UpperSecondary = 2,
    [Description("Post secondary/tertiary")] 
    PostSecondaryTertiary = 3,
    [Description("Bachelors or equivalent")]
    BachelorsOrEquivalent = 4,
    [Description("Master’s or equivalent")]
    MastersOrEquivalent = 5,
    [Description("Doctoral or equivalent")] 
    DoctoralOrEquivalent = 6
}

