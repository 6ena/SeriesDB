using Newtonsoft.Json;
using SeriesDB.Series;
using System;
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
        private static readonly string apiKey = "39e61792"; // Reemplaza con tu clave API de OMDb.
        private static readonly string baseUrl = "http://www.omdbapi.com/";

        public async Task<ICollection<SerieDto>> BuscarSerieAsync(string titulo, string genero)
        {
            using HttpClient client = new HttpClient();

            List<SerieDto> series = new List<SerieDto>();

            string url = $"{baseUrl}?s={titulo}&apikey={apiKey}&type=series";

            try
            {
                // Hacer la solicitud HTTP y obtener la respuesta como string
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta JSON a un objeto SearchResponse
                var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(jsonResponse);

                // Retornar la lista de series si existen
                var seriesOmdb = searchResponse?.Search ?? new List<SerieOmdb>();

                foreach (var serieOmdb in seriesOmdb)
                {
                    series.Add(new SerieDto
                    {
                        Titulo = serieOmdb.Title,
                        //Generos = serieOmdb.Genre,
                        //Sinopsis = serieOmdb.Plot,
                        FechaEstreno = serieOmdb.Released,
                        //Duracion = serieOmdb.Runtime,
                        //Clasificacion = serieOmdb.Rated,
                        //Idiomas = serieOmdb.Language,
                        //Directores = serieOmdb.Director,
                        //Escritores = serieOmdb.Writer,
                        //Actores = serieOmdb.Actors,
                        Poster = serieOmdb.Poster,
                        //Pais = serieOmdb.Country,
                        ImdbId = serieOmdb.ImdbID,
                        //ImdbCalificacion = serieOmdb.ImdbRating,
                        //ImdbVotos = serieOmdb.ImdbVotes,
                        Tipo = serieOmdb.Type,
                        //TotalTemporadas = serieOmdb.totalSeasons,
                    });
                }

                return series;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Se ha producido un error en la búsqueda de la serie", e);
            }
        }

        private class SearchResponse
        {
            [JsonProperty("Search")]
            public List<SerieOmdb> Search { get; set; }
        }

        private class SerieOmdb
        {
            [JsonProperty("Title")]
            public string Title { get; set; }
            /*
            [JsonProperty("Genre")]
            public string Genre { get; set; }

            [JsonProperty("Plot")]
            public string Plot { get; set; }
            */
            [JsonProperty("Released")]
            public string Released { get; set; }
            /*
            [JsonProperty("Runtime")]
            public string Runtime { get; set; }

            [JsonProperty("Rated")]
            public string Rated { get; set; }

            [JsonProperty("Language")]
            public string Language { get; set; }

            [JsonProperty("Director")]
            public string Director { get; set; }

            [JsonProperty("Writer")]
            public string Writer { get; set; }

            [JsonProperty("Actors")]
            public string Actors { get; set; }
            */
            [JsonProperty("Poster")]
            public string Poster { get; set; }
            /*
            [JsonProperty("Country")]
            public string Country { get; set; }
            */
            [JsonProperty("imdbID")]
            public string ImdbID { get; set; }
            /*
            [JsonProperty("imdbRating")]
            public string ImdbRating { get; set; }

            [JsonProperty("imdbVotes")]
            public int ImdbVotes { get; set; }
            */
            [JsonProperty("Type")]
            public string Type { get; set; }
            /*
            [JsonProperty("totalSeasons")]
            public int totalSeasons {get; set;}
            */
        }
    }
}