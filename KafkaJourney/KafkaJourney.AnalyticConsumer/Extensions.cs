using KafkaJourney.AnalyticConsumer.Persistence;
using KafkaJourney.AnalyticConsumer.Services;

namespace KafkaJourney.AnalyticConsumer;

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
            .AddSystemOptions(configuration)
            .AddSystemServices()
            .AddDb<AnalyticDbContext>(configuration, "mssql")
            .AddSchemaRegistry()
            .AddConsumer<int, EventAction>()
            .BuildServiceProvider();

    public static IServiceCollection AddSystemOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)))
            .Configure<SchemaRegistryConfig>(configuration.GetSection(nameof(SchemaRegistryConfig)));

        return services;
    }

    public static IServiceCollection AddSystemServices(this IServiceCollection services)
    {
        services.AddSingleton<IVideoPlayService, VideoPlayService>();

        return services;
    }
}