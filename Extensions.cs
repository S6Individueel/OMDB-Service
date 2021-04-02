using OMDB_Service.Data.Models;
using OMDB_Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMDB_Service
{
    public static class Extensions
    {
        public static MovieTopAiringDTO AsTopDTO(this TopMovie movie)
        {
            return new MovieTopAiringDTO
            {
                adult = movie.adult,
                backdrop_path = movie.backdrop_path,
                genre_ids = movie.genre_ids,
                id = movie.id,
                overview = movie.overview,
                popularity = movie.popularity,
                poster_path = movie.poster_path,
                release_date = movie.release_date,
                title = movie.title,
                vote_average = movie.vote_average
            };
        }
    }
}
