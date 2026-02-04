using System.Threading.Tasks;

namespace SeriesDB.Series
{
    public interface ISerieUpdateService
    {
        Task VerificarYActualizarSeriesAsync();
    }
}