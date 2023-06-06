using KafkaJourney.JourneyConsumer.Entities;
using Microsoft.EntityFrameworkCore;

namespace KafkaJourney.JourneyConsumer.Persistence;

internal sealed class JourneyDbContext : DbContext
{
    public JourneyDbContext(DbContextOptions<JourneyDbContext> options) : base(options)
    {
    }

    public DbSet<EventLogEntry> EventLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventLogEntry>(builder =>
        {
            builder.ToTable("EventLogs");
            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.EventName).IsRequired();
            builder.Property(e => e.EventType).IsRequired();
            builder.Property(e => e.EventIssuedAt).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.TransactionId).IsRequired();
        });
    }
}