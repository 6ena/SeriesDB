using Microsoft.Extensions.Hosting;
using SeriesDB.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;


namespace SeriesDB.ListasDeSeguimiento
{
    public class ListaDeSeguimientoAppService : ApplicationService, IListaDeSeguimientoAppService
    {

        private readonly IRepository<ListaDeSeguimiento, int> _listaDeSeguimientoRepository;
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly ICurrentUser _currentUser;

        public ListaDeSeguimientoAppService(
            IRepository<ListaDeSeguimiento, int> listaDeSeguimientoRepository,
            IRepository<Serie, int> serieRepository,
            ICurrentUser currentUser)
        {
            _listaDeSeguimientoRepository = listaDeSeguimientoRepository;
            _serieRepository = serieRepository;
            _currentUser = currentUser;
        }

        public async Task<SerieDto[]> GetSeriesListaAsync()
        {
            //Hay que estar logueado 
            Guid userId = (Guid)_currentUser.Id;

            // Cargar la lista con la colección de Series incluida
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var listaDeSeguimiento = queryable.FirstOrDefault(l => l.IdUsuario == userId);

            // Si no existe, devolver array vacío en lugar de lanzar excepción
            if (listaDeSeguimiento == null)
            {
                return Array.Empty<SerieDto>();
            }

            return ObjectMapper.Map<Serie[], SerieDto[]>(listaDeSeguimiento.Series.ToArray());
        }


        public async Task AddSerieListaAsync(SerieDto serieDto)
        {
            //Hay que estar logueado 
            Guid userId = (Guid)_currentUser.Id;

            // Cargar la lista con la colección de series incluida
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var listaDeSeguimiento = queryable.FirstOrDefault(l => l.IdUsuario == userId);

            if (listaDeSeguimiento == null)
            {
                listaDeSeguimiento = new ListaDeSeguimiento()
                {
                    IdUsuario = userId,
                    FechaModificacion = DateOnly.FromDateTime(DateTime.Now),
                };
                await _listaDeSeguimientoRepository.InsertAsync(listaDeSeguimiento);
            }

            if (listaDeSeguimiento.Series.Any(s => s.ImdbId == serieDto.ImdbId))
            {
                throw new Exception($"La serie ya está en la lista de seguimiento del usuario.");
            }
            else
            {
                // Buscar si la serie ya existe en la base de datos
                var serieExistente = (await _serieRepository.GetListAsync())
                    .FirstOrDefault(s => s.ImdbId == serieDto.ImdbId);

                if (serieExistente == null)
                {
                    // Si no existe se crea
                    var nuevaSerie = ObjectMapper.Map<SerieDto, Serie>(serieDto);
                    serieExistente = await _serieRepository.InsertAsync(nuevaSerie);
                }

                listaDeSeguimiento.Series.Add(serieExistente);
                listaDeSeguimiento.FechaModificacion = DateOnly.FromDateTime(DateTime.Now);
            }

            await _listaDeSeguimientoRepository.UpdateAsync(listaDeSeguimiento);
        }

        //La idea principal es esta.
        //Creo tendrá más sentido cuando se implemente la interfaz gráfica.
        //Pero por ahora se implementará para quitar por ImdbId
        /*
        public async Task RemoveSerieListaAsync(SerieDto serieDto)
        {
            //Hay que estar logueado 
            Guid userId = (Guid)_currentUser.Id;

            // Cargar la lista con la colección de Series incluida
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var listaDeSeguimiento = queryable.FirstOrDefault(l => l.IdUsuario == userId);

            if (listaDeSeguimiento == null)
            {
                throw new Exception("La lista de seguimiento del usuario no existe.");
            }
            
            var serieToRemove = listaDeSeguimiento.Series.FirstOrDefault(s => s.ImdbId == serieDto.ImdbId);
            if (serieToRemove == null)
            {
                throw new Exception("La serie no está en la lista de seguimiento del usuario.");
            }
            
            listaDeSeguimiento.Series.Remove(serieToRemove);
            listaDeSeguimiento.FechaModificacion = DateOnly.FromDateTime(DateTime.Now);
            await _listaDeSeguimientoRepository.UpdateAsync(listaDeSeguimiento);
        }
        */

        //Remover por ImdbId
        public async Task RemoveSerieListaAsync(string ImdbId)
        {
            //Hay que estar logueado 
            Guid userId = (Guid)_currentUser.Id;

            // Cargar la lista con la colección de Series incluida
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var listaDeSeguimiento = queryable.FirstOrDefault(l => l.IdUsuario == userId);

            if (listaDeSeguimiento == null)
            {
                throw new Exception("La lista de seguimiento del usuario no existe.");
            }

            var serieToRemove = listaDeSeguimiento.Series.FirstOrDefault(s => s.ImdbId == ImdbId);
            if (serieToRemove == null)
            {
                throw new Exception("La serie no está en la lista de seguimiento del usuario.");
            }

            listaDeSeguimiento.Series.Remove(serieToRemove);
            listaDeSeguimiento.FechaModificacion = DateOnly.FromDateTime(DateTime.Now);
            await _listaDeSeguimientoRepository.UpdateAsync(listaDeSeguimiento);
        }


        public async Task<SerieDto[]> BuscarSeriesListaAsync(string titulo, string genero = null)
        {
            //Hay que estar logueado 
            Guid userId = (Guid)_currentUser.Id;

            //Lista con la colección de Series seguidas
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var listaDeSeguimiento = queryable.FirstOrDefault(l => l.IdUsuario == userId);

            if (listaDeSeguimiento == null)
            {
                return Array.Empty<SerieDto>();
            }

            var seriesListaEncontradas = new List<SerieDto>();

            if (!string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(genero))
            {
                foreach (var serie in listaDeSeguimiento.Series)
                {
                    if (serie.Titulo != null && serie.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                    {
                        seriesListaEncontradas.Add(ObjectMapper.Map<Serie, SerieDto>(serie));
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(genero))
            {
                foreach (var serie in listaDeSeguimiento.Series)
                {
                    if (serie.Generos != null && serie.Generos.Contains(genero, StringComparison.OrdinalIgnoreCase))
                    {
                        seriesListaEncontradas.Add(ObjectMapper.Map<Serie, SerieDto>(serie));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(genero))
            {
                foreach (var serie in listaDeSeguimiento.Series)
                {
                    if (serie.Titulo != null && serie.Generos != null &&
                        serie.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase) &&
                        serie.Generos.Contains(genero, StringComparison.OrdinalIgnoreCase))
                    {
                        seriesListaEncontradas.Add(ObjectMapper.Map<Serie, SerieDto>(serie));
                    }
                }
            }

            return seriesListaEncontradas.ToArray();
        }

    }

}

