using Volo.Abp.Settings;

namespace WIZLOG.Settings;

public class WIZLOGSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(WIZLOGSettings.MySetting1));
    }
}
