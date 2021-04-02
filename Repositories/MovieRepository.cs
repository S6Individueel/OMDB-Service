﻿using Newtonsoft.Json.Linq;
using OMDB_Service.Data.Models;
using OMDB_Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OMDB_Service.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly HttpClient _httpclient;
        public MovieRepository(HttpClient httpClient)
        {
            _httpclient = httpClient;
        }

        public async Task<IEnumerable<TopMovie>> GetTopTenAsync()
        {
            string uri = _httpclient.BaseAddress + "/movie/now_playing?api_key=3caffe903f7c34234eb189d6db9544fc&language=en-US&page=1"; // TODO: Getkey from encrypted environment variable sometime later
            HttpResponseMessage response = await _httpclient.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();

            IList<JToken> results = JObject.Parse(content)["results"].Children().ToList(); //Parses content, gets the "top" list and converts to list.

            IList<TopMovie> topMovies = new List<TopMovie>();
            foreach (JToken movie in results)
            {
                TopMovie topAnime = movie.ToObject<TopMovie>();
                topMovies.Add(topAnime);
            }

            return topMovies;
        }
    }
}