using Volo.Abp.Modularity;

namespace WIZLOG;

[DependsOn(
    typeof(WIZLOGApplicationModule),
    typeof(WIZLOGDomainTestModule)
    )]
public class WIZLOGApplicationTestModule : AbpModule
{

}
