using OMDB_Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMDB_Service.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<TopMovie>> GetTopTenAsync();
    }
}
