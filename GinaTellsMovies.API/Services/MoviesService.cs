using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static GinaTellsMovies.API.Properties.ConfigApi;

namespace GinaTellsMovies.API.Services
{
    public interface  IMoviesService
    {
        Task<int> SearchKeywordId(string keyword);
    }

    public class MoviesService  : IMoviesService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<int> SearchKeywordId(string keyword)
        {
            string responseString;
            JObject responseJson = null;
            const string api_key = ApiToken;
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/search/keyword?api_key={api_key}&query={keyword}");
                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    responseJson = JObject.Parse(responseString);
                    return Int32.Parse(responseJson["results"][0]["id"].ToString());
                }
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
