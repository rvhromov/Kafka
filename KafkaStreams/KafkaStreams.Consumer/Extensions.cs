using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using KafkaStreams.Consumer.Handlers;
using KafkaStreams.Consumer.Options;

namespace KafkaStreams.Consumer;

public static class Extensions
{
    public static IConfiguration GetConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

    public static IServiceProvider GetServices(IConfiguration configuration) =>
        new ServiceCollection()
            .AddSingleton(configuration)
            .AddSingleton(new State())
            .AddKafka(kafka =>
            {
                var teamOpt = configuration
                    .GetSection(nameof(TeamPossessionConsumerOptions))
                    .Get<TeamPossessionConsumerOptions>();

                kafka.AddCluster(cluster => cluster
                    .WithBrokers(new[] { Constants.BootstrapServers })

                    .CreateTopicIfNotExists(Constants.TeamPossessionTopic, teamOpt.NumberOfPartitions, teamOpt.ReplicationFactor)
                    .AddConsumer(consumer => consumer
                        .Topic(Constants.TeamPossessionTopic)
                        .WithGroupId(teamOpt.GroupId)
                        .WithBufferSize(teamOpt.BufferSize)
                        .WithWorkersCount(teamOpt.NumberOfPartitions)
                        .AddMiddlewares(m => m
                            .AddSerializer<JsonCoreSerializer>()
                            .AddTypedHandlers(h => h.AddHandler<TeamPossessionHandler>()))));
            })
            .BuildServiceProvider();
}