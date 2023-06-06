namespace KafkaStreams.Producer;

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
            .AddOptions(configuration)
            .AddSystemServices()
            .AddKafka(kafka =>
            {
                var lineupProducerOpt = configuration.GetSection(nameof(LineupProducerOptions)).Get<LineupProducerOptions>();
                var passProducerOpt = configuration.GetSection(nameof(PassProducerOptions)).Get<PassProducerOptions>();

                kafka.AddCluster(cluster => cluster
                    .WithBrokers(new[] { Constants.BootstrapServers })

                    .CreateTopicIfNotExists(Constants.LineupTopic, lineupProducerOpt.NumberOfPartitions, lineupProducerOpt.ReplicationFactor)
                    .AddProducer(lineupProducerOpt.Name, producer => producer
                        .DefaultTopic(Constants.LineupTopic)
                        .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>()))

                    .CreateTopicIfNotExists(Constants.PassesTopic, passProducerOpt.NumberOfPartitions, passProducerOpt.ReplicationFactor)
                    .AddProducer(passProducerOpt.Name, producer => producer
                        .DefaultTopic(Constants.PassesTopic)
                        .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())));
            })
            .BuildServiceProvider();

    public static IMessageProducer GetProducer<T>(IServiceProvider services) where T : ProducerOptions
    {
        var options = services.GetRequiredService<IOptions<T>>();

        return services
            .GetRequiredService<IProducerAccessor>()
            .GetProducer(options.Value.Name);
    }

    private static IServiceCollection AddSystemServices(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<LineupProducerOptions>(configuration.GetSection(nameof(LineupProducerOptions)))
            .Configure<PassProducerOptions>(configuration.GetSection(nameof(PassProducerOptions)));

        return services;
    }
}
