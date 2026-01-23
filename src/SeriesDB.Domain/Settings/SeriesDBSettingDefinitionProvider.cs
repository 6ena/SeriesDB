using Volo.Abp.Settings;

namespace SeriesDB.Settings;

public class SeriesDBSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SeriesDBSettings.MySetting1));
    }
}
