using Microsoft.AspNetCore.Mvc;
using OMDB_Service.DTOs;
using OMDB_Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMDB_Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet("popular")]
        public async Task<IEnumerable<MovieTopAiringDTO>> GetTopTenAsync()
        {
            var movies = (await _movieRepository.GetTopTenAsync())
                            .Select(movie => movie.AsTopDTO());
            return movies;
        }
    }
}
