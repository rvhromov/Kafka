using KafkaJourney.AnalyticConsumer.Services;
using Extensions = KafkaJourney.AnalyticConsumer.Extensions;

/*
 * This consumer is only interested in VideoPlayed events of each user
 * */

IConfiguration configuration = Extensions.GetConfiguration();
IServiceProvider services = Extensions.GetServices(configuration);

var consumer = services.GetService<IConsumer<int, EventAction>>();
var videoPlayService = services.GetService<IVideoPlayService>();
var cts = CancellationTokenService.Create();

Console.WriteLine("Press Ctrl+C to stop the Analytic consumer");
Console.WriteLine();

consumer.Subscribe(Constants.UserJourneyTopic);

while (!cts.Token.IsCancellationRequested)
{
    try
    {
        var result = consumer.Consume(cts.Token);

        if (result.Message.Value.Event != EventType.VideoPlayed)
        {
            continue;
        }

        Console.WriteLine($"New event: {result.Message.Key} - {result.Message.Value.Event}");

        await videoPlayService.UpdateVideoPlayAsync(result.Message.Key);

        // Manually store the offset after successful result processing (at-least-once delivery)
        consumer.StoreOffset(result);
    }
    catch (Exception)
    {
        cts.Cancel();
        consumer.Close();
    }
}