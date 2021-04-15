using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static GinaTellsMovies.API.Properties.ConfigApi;

namespace GinaTellsMovies.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class MoviesController : ControllerBase
    {

        private readonly HttpClient _client = new HttpClient();

        [HttpGet("PopularMovies")]
        public async Task<IActionResult> PopularMovies()
        {
            string movieString;
            object movieJson = null;
            const string api_key = ApiToken;
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={api_key}");
                if (response.IsSuccessStatusCode)
                {
                    movieString = await response.Content.ReadAsStringAsync();
                    movieJson = JToken.Parse(movieString).ToObject<object>();
                    return Ok(movieJson);
                }
                return StatusCode(400);
            }catch (Exception ex){
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

    }
}
