using GinaTellsMovies.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GinaTellsMovies.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AnswerItem
    {
        public string Keyword { get; set; }
        public int Weight { get; set; }
    }

    public class Answer
    {
        public List<AnswerItem> Keywords { get; set; }
    }

    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet("popularMovies")]
        public async Task<IActionResult> PopularMovies()
        {
            var movieJson = await _moviesService.GetPopularMovies();
            return Ok(movieJson);
            
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchMoviesAsync(
            [FromBody] Answer answer)
        {
            return Ok(await _moviesService.SearchMoviesAsync(answer));
        }
    }
}
