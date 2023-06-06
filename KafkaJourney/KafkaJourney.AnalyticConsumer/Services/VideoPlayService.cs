using KafkaJourney.AnalyticConsumer.Entities;
using KafkaJourney.AnalyticConsumer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KafkaJourney.AnalyticConsumer.Services;

internal sealed class VideoPlayService : IVideoPlayService
{
    private readonly AnalyticDbContext _dbContext;

    public VideoPlayService(AnalyticDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task UpdateVideoPlayAsync(int userId)
    {
        var videoPlay = await _dbContext.VideoPlays.FirstOrDefaultAsync(v => v.UserId == userId);

        if (videoPlay is null)
        {
            var newVideoPlay = new VideoPlay(userId);

            _dbContext.Add(newVideoPlay);
            await _dbContext.SaveChangesAsync();

            return;
        }

        videoPlay.IncrementCount();

        await _dbContext.SaveChangesAsync();
    }
}
