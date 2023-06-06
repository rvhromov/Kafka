using KafkaJourney.JourneyConsumer.Entities;
using KafkaJourney.JourneyConsumer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KafkaJourney.Consumer.UserJourney.Services;

internal sealed class EventLogEntryService : IEventLogEntryService
{
    private readonly JourneyDbContext _dbContext;

    public EventLogEntryService(JourneyDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task AddAsync(EventAction @event)
    {
        await ResilientTransaction
            .CreateNew(_dbContext)
            .ExecuteAsync(async () =>
            {
                var currentTransaction = _dbContext.Database.CurrentTransaction;

                var eventLogEntry = new EventLogEntry(@event.Event, @event.CreatedAt, @event.UserId, currentTransaction.TransactionId);

                _dbContext.Database.UseTransaction(currentTransaction.GetDbTransaction());
                _dbContext.EventLogEntries.Add(eventLogEntry);

                await _dbContext.SaveChangesAsync();
            });
    }
}