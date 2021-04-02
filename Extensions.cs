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
                overview = movie.overview,
                release_date = movie.release_date,
                adult = movie.adult,
                backdrop_path = movie.backdrop_path,
                vote_count = movie.vote_count,
                genre_ids = movie.genre_ids,
                vote_average = movie.vote_average,
                original_language = movie.original_language,
                poster_path = movie.poster_path,
                title = movie.title,
                id = movie.id,
                media_type = movie.media_type,
            };
        }
    }
}
