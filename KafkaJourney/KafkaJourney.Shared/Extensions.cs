using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KafkaJourney.Shared;

public static class Extensions
{
    public static IServiceCollection AddConsumer<TKey, TValue>(this IServiceCollection services)
        where TKey : struct
        where TValue : class
    {
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<ConsumerConfig>>();
            config.Value.ClientId = $"{config.Value.ClientId}-{Guid.NewGuid()}";

            return new ConsumerBuilder<TKey, TValue>(config.Value)
                .SetValueDeserializer(new JsonDeserializer<TValue>().AsSyncOverAsync())
                .Build();
        });

        return services;
    }

    public static IServiceCollection AddProducer<TKey, TValue>(this IServiceCollection services)
        where TKey : struct
        where TValue : class
    {
        services.AddSingleton(sp =>
        {
            var producerConfig = sp.GetRequiredService<IOptions<ProducerConfig>>();
            var schemaRegistry = sp.GetRequiredService<ISchemaRegistryClient>();
            producerConfig.Value.ClientId = $"{producerConfig.Value.ClientId}-{Guid.NewGuid()}";

            return new ProducerBuilder<TKey, TValue>(producerConfig.Value)
                .SetValueSerializer(new JsonSerializer<TValue>(schemaRegistry).AsSyncOverAsync())
                .Build();
        });

        return services;
    }

    public static IServiceCollection AddSchemaRegistry(this IServiceCollection services)
    {
        services.AddSingleton<ISchemaRegistryClient>(sp =>
        {
            var schemaConfig = sp.GetRequiredService<IOptions<SchemaRegistryConfig>>();

            return new CachedSchemaRegistryClient(schemaConfig.Value);
        });

        return services;
    }

    public static IServiceCollection AddDb<TContext>(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        where TContext : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        services.AddDbContext<TContext>(opt => opt.UseSqlServer(connectionString, x => x.EnableRetryOnFailure()));

        return services;
    }
}
