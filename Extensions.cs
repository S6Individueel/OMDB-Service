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
/*                Id = anime.mal_id,
                Name = anime.title,
                ImageUrl = anime.image_url,
                Rank = anime.rank,
                Score = anime.score,
                StartDate = anime.start_date,
                EndDate = anime.end_date,
                MaxEpisodes = anime.episodes.Equals(null) ? "Unknown" : anime.episodes.Value.ToString()*/
            };
        }
    }
}
