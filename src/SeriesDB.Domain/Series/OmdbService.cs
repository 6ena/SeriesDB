using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeriesDB.Series;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeriesDB.Series
{
    public class OmdbService : ISeriesApiService
    {
        private const string apiKey = "39e61792"; // Reemplaza con tu clave API de OMDb.
        private const string baseUrl = "http://www.omdbapi.com/";

        // Método principal de búsqueda que controla la lógica de validación
        public async Task<SerieDto[]> BuscarSerieAsync(string titulo, string genero = null)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("Se requiere titulo para la búsqueda.", nameof(titulo));
            }

            if (!string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(genero))
            {
                return await BuscarSeriePorTituloAsync(titulo);
            }

            if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrWhiteSpace(genero))
            {
                return await BuscarSeriePorTituloYGeneroAsync(titulo, genero);
            }

            throw new ArgumentException("No es posible buscar solo por género, titulo es obligatorio.");
        }


        // Busquedas
        private async Task<SerieDto[]> BuscarSeriePorTituloAsync(string titulo)
        {
            var url = $"{baseUrl}?apikey={apiKey}&s={titulo}&type=series";
            return await GetSeriesAsync(url);
        }

        // Filtrar series por genero después de obtener los resultados por título
        private async Task<SerieDto[]> BuscarSeriePorTituloYGeneroAsync(string titulo, string genero)
        {
            var url = $"{baseUrl}?apikey={apiKey}&s={titulo}&type=series";
            var series = await GetSeriesAsync(url);

            var seriesFiltradas = new List<SerieDto>();
            foreach (var serie in series)
            {
                if (serie.Generos != null && serie.Generos.Contains(genero, StringComparison.OrdinalIgnoreCase))
                {
                    seriesFiltradas.Add(serie);
                }
            }
            return seriesFiltradas.ToArray();
        }


        // Método para obtener las series con sus detalles completos, usando sus Id de IMDb
        private async Task<SerieDto[]> GetSeriesAsync(string url)
        {

            using HttpClient client = new HttpClient();
            {

                // Hacer la solicitud HTTP y obtener la respuesta como string
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                if (json["Response"]?.ToString() == "False")
                {
                    return Array.Empty<SerieDto>();
                }

                var seriesJson = json["Search"];
                if (seriesJson == null)
                {
                    return Array.Empty<SerieDto>();
                }

                var seriesLista = new List<SerieDto>();
                foreach (var serie in seriesJson)
                {
                    var serieId = serie["imdbID"]?.ToString();
                    var serieDetalles = await GetDetallesSerieAsync(serieId);

                    if (serieDetalles != null)
                    {
                        seriesLista.Add(serieDetalles);
                    }
                }

                return seriesLista.ToArray();
            }
        }


        private async Task<SerieDto> GetDetallesSerieAsync(string imdbId)
        {
            var url = $"{baseUrl}?apikey={apiKey}&i={imdbId}";

            using HttpClient client = new HttpClient();
            {
                 // Hacer la solicitud HTTP y obtener la respuesta como string
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                return new SerieDto
                {
                    Titulo = json["Title"]?.ToString(),
                    Clasificacion = json["Rated"]?.ToString(),
                    FechaEstreno = json["Released"]?.ToString(),
                    Duracion = json["Runtime"]?.ToString(),
                    Generos = json["Genre"]?.ToString(),
                    Directores = json["Director"]?.ToString(),
                    Escritores = json["Writer"]?.ToString(),
                    Actores = json["Actors"]?.ToString(),
                    Sinopsis = json["Plot"]?.ToString(),
                    Idiomas = json["Language"]?.ToString(),
                    Pais = json["Country"]?.ToString(),
                    Poster = json["Poster"]?.ToString(),
                    ImdbId = imdbId,
                    ImdbCalificacion = json["imdbRating"]?.ToString(),
                    ImdbVotos = int.TryParse(json["imdbVotes"]?.ToString().Replace(",", ""), out var votes) ? votes : 0,
                    Tipo = json["Type"]?.ToString(),
                    TotalTemporadas = int.TryParse(json["totalSeasons"]?.ToString(), out var seasons) ? seasons : 0
                };
            }
        }
    }
}