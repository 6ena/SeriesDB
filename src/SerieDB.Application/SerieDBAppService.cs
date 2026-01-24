using SerieDB.Localization;
using Volo.Abp.Application.Services;

namespace SerieDB;

/* Inherit your application services from this class.
 */
public abstract class SerieDBAppService : ApplicationService
{
    protected SerieDBAppService()
    {
        LocalizationResource = typeof(SerieDBResource);
    }
}
