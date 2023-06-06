namespace KafkaJourney.Shared.Models;

public class EventAction
{
    public int UserId { get; set; }
    public EventType Event { get; set; }
    public DateTime CreatedAt { get; set; }
}
