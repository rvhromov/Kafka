using KafkaJourney.Consumer.UserJourney.Services;
using Extensions = KafkaJourney.Consumer.UserJourney.Extensions;

/*
 * This consumer collects each event in the order they were produced by a user
 * */

IConfiguration configuration = Extensions.GetConfiguration();
IServiceProvider services = Extensions.GetServices(configuration);

var consumer = services.GetService<IConsumer<int, EventAction>>();
var eventLogService = services.GetService<IEventLogEntryService>();
var cts = CancellationTokenService.Create();

Console.WriteLine("Press Ctrl+C to stop the Journey consumer");
Console.WriteLine();

consumer.Subscribe(Constants.UserJourneyTopic);

while (!cts.Token.IsCancellationRequested)
{
    try
    {
        var result = consumer.Consume(cts.Token);
        Console.WriteLine($"New event: {result.Message.Key} - {result.Message.Value.Event}");

        eventLogService.AddAsync(result.Message.Value);

        // Manually store the offset after successful result processing (at-least-once delivery)
        consumer.StoreOffset(result);
    }
    catch (Exception)
    {
        cts.Cancel();
        consumer.Close();
    }
}
