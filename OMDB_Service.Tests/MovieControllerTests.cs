using FakeItEasy;
using System;
using Xunit;
using OMDB_Service.Repositories.Interfaces;
using OMDB_Service.Controllers;
using OMDB_Service.Data.Models;
using System.Threading.Tasks;
using System.Linq;
using OMDB_Service.DTOs;

namespace OMDB_Service.Tests
{
    public class MovieControllerTests
    {
        [Fact]
        public async Task Get_Top_Ten_Movies()
        {
            //Arange
            int movieCount = 10;
            var movieRepo = A.Fake<IMovieRepository>(); //Fakes a repository to initialize the controller with
            var fakeMovies = A.CollectionOfDummy<TopMovie>(10).AsEnumerable(); //Makes a dummy collection of the topmovietype
            A.CallTo(() => movieRepo.GetTopTenAsync()).Returns(await Task.FromResult(fakeMovies));//Configures the call to return the faked data, making it independent from the API and testing pure code.
            var controller = new MovieController(movieRepo);
            //Act

            var result = await controller.GetTopTenAsync(); //Makes the call
            //Assert

            Assert.Equal(movieCount, result.Count()); //Checks if the list is filled
        }
    }
}
