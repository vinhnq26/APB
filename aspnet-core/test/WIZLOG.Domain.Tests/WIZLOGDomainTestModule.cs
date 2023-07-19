using WIZLOG.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace WIZLOG;

[DependsOn(
    typeof(WIZLOGEntityFrameworkCoreTestModule)
    )]
public class WIZLOGDomainTestModule : AbpModule
{

}
