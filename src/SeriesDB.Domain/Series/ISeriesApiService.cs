using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriesDB.Series
{
    public interface ISeriesApiService
    {
        Task<SerieDto[]> BuscarSerieAsync(string titulo, string genero);
    }
}
