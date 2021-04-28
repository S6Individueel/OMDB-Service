using OMDB_Service.Data.Models;
using OMDB_Service.Rabbit.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OMDB_Service;
using OMDB_Service.Data.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMDB_Service.Rabbit
{
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly double hoursTillUpdate = 12; 
        private readonly ILogger _logger;

/*        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection _connection;*/
        private readonly HttpClient _httpclient;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, HttpClient httpClient)
        {
/*            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _connection = connectionFactory.CreateConnection();*/
            _httpclient = httpClient;

            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_exchange",                     //EXCHANGE creation
                                            type: "topic");

                    string uri = "https://api.themoviedb.org/3" + "/trending/movie/week?api_key=3caffe903f7c34234eb189d6db9544fc";
                    
                    HttpResponseMessage response = await _httpclient.GetAsync(uri);
                    var content = await response.Content.ReadAsStringAsync();

                    IList<JToken> results = JObject.Parse(content)["results"].Children().ToList(); //Parses content, gets the "top" list and converts to list.

                    IList<TopMovie> topMovies = new List<TopMovie>();
                    foreach (JToken movie in results)
                    {
                        TopMovie topAnime = movie.ToObject<TopMovie>();
                        topMovies.Add(topAnime);
                    }
                    var showList = topMovies.Select(movie => movie.AsShowDTO());

                    var json = JsonConvert.SerializeObject(showList);                    //MESSAGE creation
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "topic_exchange",                        //MESSAGE publishing
                                         routingKey: "shows.movie.trending",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent Update message", body);
                }
                await Task.Delay(TimeSpan.FromHours(hoursTillUpdate), stoppingToken);
            }
        }
    }
}
