using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum TypeOfSecondarySchool
{
    [Description("Prefer not to say")] 
    PreferNotToSay = 1,
    [Description("Non-selective state funded")] 
    NonSelectiveStateFunded = 2,
    [Description("Selective (on academic, faith or other grounds) state funded school")] 
    SelectiveStateFundedSchool = 3,
    [Description("Privately funded school")] 
    PrivatelyFundedSchool = 4,
    [Description("Other – such as home schooled")] 
    Other = 5
}
