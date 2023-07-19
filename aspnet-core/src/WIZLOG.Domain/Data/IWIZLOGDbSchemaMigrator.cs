using System.Threading.Tasks;

namespace WIZLOG.Data;

public interface IWIZLOGDbSchemaMigrator
{
    Task MigrateAsync();
}
