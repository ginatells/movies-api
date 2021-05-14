using GinaTellsMovies.API.Controllers;
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
        Task<List<int>> SearchKeywordListIds(List<AnswerItem> keywords);
    }

    public class MoviesService  : IMoviesService
    {
        private readonly HttpClient _client = new HttpClient();

        //Receive keyword string and returns keyword ID
        public async Task<int> SearchKeywordId(string keyword)
        {
            Console.WriteLine(keyword);
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
        
        //Receive Keyword list and returns list with keyword ids
        public async Task<List<int>> SearchKeywordListIds(List<AnswerItem> keywords)
        {
            List<int> response = new List<int>();
            for (int i = 0; i < keywords.Count; i++)
            {
                response.Add(await SearchKeywordId(keywords.ElementAt(i).keyword));
            }
            return response;
        }
    }
}
