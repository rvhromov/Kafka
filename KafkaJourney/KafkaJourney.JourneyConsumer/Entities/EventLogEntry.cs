namespace KafkaJourney.JourneyConsumer.Entities;

public class EventLogEntry
{
    public EventLogEntry(EventType eventType, DateTime eventIssuedAt, int userId, Guid transactionId)
    {
        EventId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        EventName = eventType.ToString();
        EventType = eventType;
        EventIssuedAt = eventIssuedAt;
        UserId = userId;
        TransactionId = transactionId;
    }

    public Guid EventId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string EventName { get; private set; }
    public EventType EventType { get; private set; }
    public DateTime EventIssuedAt { get; private set; }
    public int UserId { get; private set; }
    public Guid TransactionId { get; private set; }
}
