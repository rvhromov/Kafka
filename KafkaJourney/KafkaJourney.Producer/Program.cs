using KafkaJourney.Producer.Services;
using Extensions = KafkaJourney.Producer.Extensions;

IConfiguration configuration = Extensions.GetConfiguration();
IServiceProvider services = Extensions.GetServices(configuration);

var producer = services.GetService<IProducer<int, EventAction>>();
var schema = services.GetService<ISchemaRegistryClient>();

Console.WriteLine("Press Esc to stop the Producer");
Console.WriteLine();

while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
{
    var message = MessageGenerator.GetNextMessage();
    producer.Produce(Constants.UserJourneyTopic, message, DeliveryHandlers.HandleError);

    Console.WriteLine($"Event sent: {message.Key} - {message.Value.Event}");
    await Task.Delay(TimeSpan.FromSeconds(1));
}

producer.Flush(TimeSpan.FromSeconds(10));