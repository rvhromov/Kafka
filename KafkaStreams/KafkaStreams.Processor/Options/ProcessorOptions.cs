namespace KafkaStreams.Processor.Options;

public sealed record ProcessorOptions
{
    public string ApplicationId { get; init; }
    public string ClientId { get; init; }
    public string AutoOffsetReset { get; init; }
}
