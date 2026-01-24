using SeriesDB.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SeriesDB.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SeriesDBController : AbpControllerBase
{
    protected SeriesDBController()
    {
        LocalizationResource = typeof(SeriesDBResource);
    }
}
