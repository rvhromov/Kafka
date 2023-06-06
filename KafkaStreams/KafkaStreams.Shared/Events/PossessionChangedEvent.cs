namespace KafkaStreams.Shared.Events;

public sealed class PossessionChangedEvent
{
    public Guid PlayerId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; }
    public int TeamTotalPasses { get; set; }
    public DateTime TimeStamp { get; set; }
}