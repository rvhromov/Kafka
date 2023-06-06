using KafkaJourney.AnalyticConsumer.Entities;
using Microsoft.EntityFrameworkCore;

namespace KafkaJourney.AnalyticConsumer.Persistence;

internal sealed class AnalyticDbContext : DbContext
{
    public AnalyticDbContext(DbContextOptions<AnalyticDbContext> options) : base(options)
    {
    }

    public DbSet<VideoPlay> VideoPlays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoPlay>(builder =>
        {
            builder.ToTable("VideoPlays");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.Count).IsRequired();
        });
    }
}
