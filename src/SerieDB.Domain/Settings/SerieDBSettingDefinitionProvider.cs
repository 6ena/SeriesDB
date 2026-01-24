using Volo.Abp.Settings;

namespace SerieDB.Settings;

public class SerieDBSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SerieDBSettings.MySetting1));
    }
}
