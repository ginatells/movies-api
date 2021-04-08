using GinaTellsMovies.API.Properties;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static GinaTellsMovies.API.Properties.ConfigApi;

namespace GinaTellsMovies.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class MoviesController : ControllerBase
    {

        public class Movie
        {
            public string Name { get; set; }
        }

        private readonly HttpClient _client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> GetMovie()
        {
            Movie movie = null;
            string movieString = "";
            object movieJson = null;
            const string api_key = ApiToken;
            HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={api_key}");
            if (response.IsSuccessStatusCode)
            {
                movieString = await response.Content.ReadAsStringAsync();
                //movieJson = JsonConvert.DeserializeObject(movieString);
                movieJson = JToken.Parse(movieString).ToObject<object>();
            }
            return Ok(movieJson);
        }
    }
}
