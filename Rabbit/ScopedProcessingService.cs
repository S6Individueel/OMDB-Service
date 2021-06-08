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
using OMDB_Service.Repositories.Interfaces;

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
        private IMovieRepository _movieRepository;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, HttpClient httpClient, IMovieRepository movieRepository)
        {
/*            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _connection = connectionFactory.CreateConnection();*/
            _httpclient = httpClient;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                var factory = new ConnectionFactory() { HostName = "rabbitmq" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_exchange",                     //EXCHANGE creation
                                            type: "topic");

                    var showList = (await _movieRepository.GetTopTenAsync())            //GetShowList
                            .Select(movie => movie.AsShowDTO());

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
