namespace KafkaJourney.Consumer.UserJourney.Services;

public interface IEventLogEntryService
{
    Task AddAsync(EventAction @event);
}
