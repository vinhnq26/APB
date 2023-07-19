using System;
using System.Collections.Generic;
using System.Text;
using WIZLOG.Localization;
using Volo.Abp.Application.Services;

namespace WIZLOG;

/* Inherit your application services from this class.
 */
public abstract class WIZLOGAppService : ApplicationService
{
    protected WIZLOGAppService()
    {
        LocalizationResource = typeof(WIZLOGResource);
    }
}
