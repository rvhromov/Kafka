namespace KafkaJourney.AnalyticConsumer.Services;

internal interface IVideoPlayService
{
    Task UpdateVideoPlayAsync(int userId);
}