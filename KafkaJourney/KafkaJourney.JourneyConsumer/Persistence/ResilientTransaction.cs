using Microsoft.EntityFrameworkCore;

namespace KafkaJourney.JourneyConsumer.Persistence;

public sealed class ResilientTransaction
{
    private readonly DbContext _context;

    private ResilientTransaction(DbContext context) =>
        _context = context;

    public static ResilientTransaction CreateNew(DbContext context) =>
        new(context);

    public async Task ExecuteAsync(Func<Task> action)
    {
        // Adds retries if there's an exception while saving to DB
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = _context.Database.BeginTransaction();

            await action();

            transaction.Commit();
        });
    }
}