using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WIZLOG.Data;
using Volo.Abp.DependencyInjection;

namespace WIZLOG.EntityFrameworkCore;

public class EntityFrameworkCoreWIZLOGDbSchemaMigrator
    : IWIZLOGDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreWIZLOGDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the WIZLOGDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<WIZLOGDbContext>()
            .Database
            .MigrateAsync();
    }
}
