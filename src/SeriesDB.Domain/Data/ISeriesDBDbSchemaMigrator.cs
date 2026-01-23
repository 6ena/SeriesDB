using System.Threading.Tasks;

namespace SeriesDB.Data;

public interface ISeriesDBDbSchemaMigrator
{
    Task MigrateAsync();
}
