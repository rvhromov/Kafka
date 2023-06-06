namespace KafkaStreams.Producer.Services.Interfaces;

public interface ITeamService
{
    Task RevealLineups(Team team);
    Team GetHomeTeamLineup();
    Team GetAwayTeamLineup();
}
