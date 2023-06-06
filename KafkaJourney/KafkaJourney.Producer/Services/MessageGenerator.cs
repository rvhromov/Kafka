namespace KafkaJourney.Producer.Services;

public static class MessageGenerator
{
    private static readonly Random _random = new();

    private static readonly int [] _userIds = new [] { 101, 103, 104, 107, 163, 211, 215, 216, 277, 301 };
    private static readonly EventType [] _events = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToArray();

    public static Message<int, EventAction> GetNextMessage()
    {
        var userId = _userIds[_random.Next(_userIds.Length)];
        var @event = _events[_random.Next(_events.Length)];

        return new Message<int, EventAction>
        {
            Key = userId,
            Value = new EventAction
            {
                UserId = userId,
                Event = @event,
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
