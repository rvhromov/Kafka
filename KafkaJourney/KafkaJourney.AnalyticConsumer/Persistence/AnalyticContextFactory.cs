using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KafkaJourney.AnalyticConsumer.Persistence;

internal sealed class AnalyticContextFactory : IDesignTimeDbContextFactory<AnalyticDbContext>
{
    public AnalyticDbContext CreateDbContext(string[] args)
    {
        var connectionString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<AnalyticDbContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.EnableRetryOnFailure());

        return new AnalyticDbContext(optionsBuilder.Options);
    }
}