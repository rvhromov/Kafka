namespace KafkaJourney.Producer;

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
            .AddSchemaRegistry()
            .AddProducer<int, EventAction>()
            .BuildServiceProvider();

    public static IServiceCollection AddSystemOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)))
            .Configure<SchemaRegistryConfig>(configuration.GetSection(nameof(SchemaRegistryConfig)));

        return services;
    }
}