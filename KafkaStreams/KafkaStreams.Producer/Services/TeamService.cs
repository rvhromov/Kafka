namespace KafkaStreams.Producer.Services;

public class TeamService : ITeamService
{
    private readonly IMessageProducer _lineupProducer;
    private readonly IOptions<LineupProducerOptions> _options;

    public TeamService(IServiceProvider services)
    {
        _options = services.GetRequiredService<IOptions<LineupProducerOptions>>();

        _lineupProducer = services
            .GetRequiredService<IProducerAccessor>()
            .GetProducer(_options.Value.Name);
    }

    public async Task RevealLineups(Team team)
    {
        Console.WriteLine($"{team.Name}'s lineup for today's match:");

        foreach (var player in team.Players)
        {
            var addedToStartingLineup = new PlayerAddedToStartingLineupEvent
            {
                TeamId = team.Id,
                TeamName = team.Name,
                PlayerId = player.Id,
                PlayerName = player.Name,
                Shirt = player.ShirtNumber
            };

            await _lineupProducer.ProduceAsync(player.Id.ToString(), addedToStartingLineup);

            Console.WriteLine($"{player.Name} {player.ShirtNumber}");
            await Task.Delay(TimeSpan.FromMilliseconds(_options.Value.IntervalBetweenEventsMs));
        }

        Console.WriteLine();
    }

    public Team GetHomeTeamLineup()
    {
        return new Team
        {
            Id = Guid.NewGuid(),
            Name = Constants.HomeTeam,
            Players = new List<Player>
            {
                new Player {Id = Guid.NewGuid(), Name = "Courtois", ShirtNumber = 1},
                new Player {Id = Guid.NewGuid(), Name = "Carvajal", ShirtNumber = 2},
                new Player {Id = Guid.NewGuid(), Name = "Rudiger", ShirtNumber = 22},
                new Player {Id = Guid.NewGuid(), Name = "Alaba", ShirtNumber = 4},
                new Player {Id = Guid.NewGuid(), Name = "Vazquez", ShirtNumber = 17},
                new Player {Id = Guid.NewGuid(), Name = "Kroos", ShirtNumber = 8},
                new Player {Id = Guid.NewGuid(), Name = "Camavinga", ShirtNumber = 12},
                new Player {Id = Guid.NewGuid(), Name = "Modric", ShirtNumber = 10},
                new Player {Id = Guid.NewGuid(), Name = "Vinicius", ShirtNumber = 20},
                new Player {Id = Guid.NewGuid(), Name = "Benzema", ShirtNumber = 9},
                new Player {Id = Guid.NewGuid(), Name = "Asensio", ShirtNumber = 11},
            }
        };
    }

    public Team GetAwayTeamLineup()
    {
        return new Team
        {
            Id = Guid.NewGuid(),
            Name = Constants.AwayTeam,
            Players = new List<Player>
            {
                new Player {Id = Guid.NewGuid(), Name = "Ter Stegen", ShirtNumber = 1},
                new Player {Id = Guid.NewGuid(), Name = "Alba", ShirtNumber = 18},
                new Player {Id = Guid.NewGuid(), Name = "Christensen", ShirtNumber = 15},
                new Player {Id = Guid.NewGuid(), Name = "Kounde", ShirtNumber = 23},
                new Player {Id = Guid.NewGuid(), Name = "Balde", ShirtNumber = 28},
                new Player {Id = Guid.NewGuid(), Name = "De Jong", ShirtNumber = 21},
                new Player {Id = Guid.NewGuid(), Name = "Busquets", ShirtNumber = 5},
                new Player {Id = Guid.NewGuid(), Name = "Kessie", ShirtNumber = 19},
                new Player {Id = Guid.NewGuid(), Name = "Dembele", ShirtNumber = 7},
                new Player {Id = Guid.NewGuid(), Name = "Lewandowski", ShirtNumber = 9},
                new Player {Id = Guid.NewGuid(), Name = "Raphinha", ShirtNumber = 22},
            }
        };
    }
}