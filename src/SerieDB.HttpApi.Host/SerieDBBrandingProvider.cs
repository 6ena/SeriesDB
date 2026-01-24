using Microsoft.Extensions.Localization;
using SerieDB.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SerieDB;

[Dependency(ReplaceServices = true)]
public class SerieDBBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SerieDBResource> _localizer;

    public SerieDBBrandingProvider(IStringLocalizer<SerieDBResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
