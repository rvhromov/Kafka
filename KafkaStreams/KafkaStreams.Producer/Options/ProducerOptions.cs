namespace KafkaStreams.Producer.Options;

public abstract record ProducerOptions
{
    public string Name { get; init; }
    public int NumberOfPartitions { get; init; }
    public short ReplicationFactor { get; init; }
    public int IntervalBetweenEventsMs { get; init; }
}