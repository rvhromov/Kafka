namespace KafkaStreams.Consumer.Options;

public sealed record TeamPossessionConsumerOptions
{
    public string GroupId { get; init; }
    public int NumberOfPartitions { get; init; }
    public short ReplicationFactor { get; init; }
    public int BufferSize { get; init; }
}
