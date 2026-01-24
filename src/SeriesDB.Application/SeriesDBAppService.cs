using SeriesDB.Localization;
using Volo.Abp.Application.Services;

namespace SeriesDB;

/* Inherit your application services from this class.
 */
public abstract class SeriesDBAppService : ApplicationService
{
    protected SeriesDBAppService()
    {
        LocalizationResource = typeof(SeriesDBResource);
    }
}
