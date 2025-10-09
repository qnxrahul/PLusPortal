using Volo.Abp.Settings;

namespace Steer73.RockIT.Settings;

public class RockITSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(RockITSettings.MySetting1));
    }
}
