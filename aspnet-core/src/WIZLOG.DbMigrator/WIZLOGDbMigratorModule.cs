using WIZLOG.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace WIZLOG.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WIZLOGEntityFrameworkCoreModule),
    typeof(WIZLOGApplicationContractsModule)
    )]
public class WIZLOGDbMigratorModule : AbpModule
{
}
