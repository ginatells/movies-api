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
    public interface IMoviesService
    {
        Task<object> GetPopularMovies();
        Task<object> SearchMoviesAsync(Answer answer);
    }

    public class MoviesService : IMoviesService
    {
        private readonly HttpClient _client = new();


        public async Task<object> GetPopularMovies()
        {
            string movieString;
            const string api_key = ApiToken;

            HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={api_key}");

            movieString = await response.Content.ReadAsStringAsync();
            object movieJson = JToken.Parse(movieString).ToObject<object>();

            return movieJson;
        }

        public async Task<object> SearchMoviesAsync(Answer answer)
        {
            string responseString;
            const string api_key = ApiToken;
            List<int> keywordList = await SearchKeywordListIds(answer.Keywords);
            string keywordIdString = "";
            for (int i = 0; i < keywordList.Count; i++)
            {
                if (i != keywordList.Count - 1)
                {
                    keywordIdString = string.Concat(keywordIdString, keywordList[i], "|");
                }
                else
                {
                    keywordIdString = string.Concat(keywordIdString, keywordList[i]);
                }
            }

            HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={api_key}&with_keywords={keywordIdString}");

            responseString = await response.Content.ReadAsStringAsync();
            object responseJson = JToken.Parse(responseString).ToObject<object>();
            return responseJson;
        }

        //Receive keyword string and returns keyword ID
        private async Task<int> SearchKeywordId(string keyword)
        {
            Console.WriteLine(keyword);
            string responseString;
            const string api_key = ApiToken;
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"https://api.themoviedb.org/3/search/keyword?api_key={api_key}&query={keyword}");
                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    JObject responseJson = JObject.Parse(responseString);
                    return int.Parse(responseJson["results"][0]["id"].ToString());
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
        private async Task<List<int>> SearchKeywordListIds(List<AnswerItem> keywords)
        {
            List<int> response = new();
            for (int i = 0; i < keywords.Count; i++)
            {
                response.Add(await SearchKeywordId(keywords.ElementAt(i).Keyword));
            }
            return response;
        }
    }
}
