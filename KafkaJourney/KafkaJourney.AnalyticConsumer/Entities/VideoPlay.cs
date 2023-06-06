namespace KafkaJourney.AnalyticConsumer.Entities;

public class VideoPlay
{
    public VideoPlay(int userId)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        Count = ++Count;
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public int UserId { get; private set; }
    public int Count { get; private set; }

    public void IncrementCount()
    {
        Count = ++Count;
        UpdatedAt = DateTime.UtcNow;
    }
}