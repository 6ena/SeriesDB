using Microsoft.Extensions.Localization;
using SeriesDB.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SeriesDB;

[Dependency(ReplaceServices = true)]
public class SeriesDBBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SeriesDBResource> _localizer;

    public SeriesDBBrandingProvider(IStringLocalizer<SeriesDBResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
