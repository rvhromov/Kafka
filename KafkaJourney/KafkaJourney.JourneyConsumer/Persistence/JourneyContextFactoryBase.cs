using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace KafkaJourney.JourneyConsumer.Persistence;

internal class JourneyContextFactory : IDesignTimeDbContextFactory<JourneyDbContext>
{
    public JourneyDbContext CreateDbContext(string[] args)
    {
        var connectionString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<JourneyDbContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.EnableRetryOnFailure());

        return new JourneyDbContext(optionsBuilder.Options);
    }
}