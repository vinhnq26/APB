using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace WIZLOG.Data;

/* This is used if database provider does't define
 * IWIZLOGDbSchemaMigrator implementation.
 */
public class NullWIZLOGDbSchemaMigrator : IWIZLOGDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
