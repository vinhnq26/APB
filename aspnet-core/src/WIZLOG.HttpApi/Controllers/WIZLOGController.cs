using WIZLOG.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WIZLOG.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class WIZLOGController : AbpControllerBase
{
    protected WIZLOGController()
    {
        LocalizationResource = typeof(WIZLOGResource);
    }
}
