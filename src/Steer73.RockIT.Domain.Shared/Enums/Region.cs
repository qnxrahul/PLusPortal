using System.ComponentModel;

namespace Steer73.RockIT.Enums
{
    public enum Region
    {
        [Description("North America")]
        NorthAmerica = 1,
        [Description("South America")]
        SouthAmerica = 2,
        [Description("Europe")]
        Europe = 3,
        [Description("MENA")]
        MENA = 4,
        [Description("Sub-Saharan Africa")]
        SubSaharanAfrica = 5,
        [Description("Asia Pacific")]
        AsiaPacific = 6,
        [Description("Australasia")]
        Australasia = 7
    }
}
