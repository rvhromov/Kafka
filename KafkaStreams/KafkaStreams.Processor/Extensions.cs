using Confluent.Kafka;
using KafkaStreams.Processor.Options;

namespace KafkaStreams.Processor;

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
            .BuildServiceProvider();

    public static StreamConfig GetStreamConfig(IConfiguration configuration)
    {
        var options = configuration
            .GetSection(nameof(ProcessorOptions))
            .Get<ProcessorOptions>();

        return new StreamConfig()
        {
            ApplicationId = options.ApplicationId,
            ClientId = options.ClientId,
            BootstrapServers = Constants.BootstrapServers,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(options.AutoOffsetReset),
            WindowStoreChangelogAdditionalRetentionMs = 300000
        };
    }
}