using SerieDB.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SerieDB.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SerieDBController : AbpControllerBase
{
    protected SerieDBController()
    {
        LocalizationResource = typeof(SerieDBResource);
    }
}
