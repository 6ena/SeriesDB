using SeriesDB.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SeriesDB.ListasDeSeguimiento
{
    public interface IListaDeSeguimientoAppService : IApplicationService
    {
        Task<SerieDto[]> GetSeriesListaAsync();
        Task AddSerieListaAsync(SerieDto serieDto);
        //Task RemoveSerieListaAsync(SerieDto serieDto);
        Task RemoveSerieListaAsync(string ImdbId);
        Task<SerieDto[]> BuscarSeriesDeListaAsync(string titulo, string genero = null); //genero = null makes the parameter optional

    }
}
