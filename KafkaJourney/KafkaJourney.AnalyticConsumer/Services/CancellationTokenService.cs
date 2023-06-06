namespace KafkaJourney.AnalyticConsumer.Services;

public static class CancellationTokenService
{
    public static CancellationTokenSource Create()
    {
        CancellationTokenSource cts = new();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        return cts;
    }
}