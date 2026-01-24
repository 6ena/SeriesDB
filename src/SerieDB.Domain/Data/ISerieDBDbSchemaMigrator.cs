using System.Threading.Tasks;

namespace SerieDB.Data;

public interface ISerieDBDbSchemaMigrator
{
    Task MigrateAsync();
}
