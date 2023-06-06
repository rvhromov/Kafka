namespace KafkaStreams.Producer.Services;

public sealed class GameService
{
    private readonly Random _random;
    private readonly LocalPlayerModel[] _players;
    private readonly IMessageProducer _passProducer;
    private readonly IOptions<PassProducerOptions> _options;

    private GameService(Team homeTeam, Team awayTeam, IServiceProvider services)
    {
        _random = new Random();

        _players = homeTeam.Players
            .Concat(awayTeam.Players)
            .Select(p => new LocalPlayerModel { Id = p.Id, Name = p.Name })
            .ToArray();

        _options = services.GetRequiredService<IOptions<PassProducerOptions>>();

        _passProducer = services
            .GetRequiredService<IProducerAccessor>()
            .GetProducer(_options.Value.Name);
    }

    public static GameService StartTheGame(Team homeTeam, Team awayTeam, IServiceProvider services) =>
        new (homeTeam, awayTeam, services);

    public async Task MakePass()
    {
        var player = _players[_random.Next(_players.Length)];
        var gotBallEvent = new PossessionChangedEvent { PlayerId = player.Id };

        await _passProducer.ProduceAsync(player.Id.ToString(), gotBallEvent);

        Console.WriteLine($"Pass by {player.Name}");
        await Task.Delay(TimeSpan.FromMilliseconds(_options.Value.IntervalBetweenEventsMs));
    }

    private class LocalPlayerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}