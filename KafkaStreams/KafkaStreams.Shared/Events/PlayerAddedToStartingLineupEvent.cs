namespace KafkaStreams.Shared.Events;

public sealed class PlayerAddedToStartingLineupEvent
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; }
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; }
    public short Shirt { get; set; }
}