namespace KafkaStreams.Consumer;

public class State
{
    private int _homeTeamPasses;
    private int _awayTeamPasses;
    private int _totalPasses;
    private DateTime _lastUpdatedAt;

    public int TotalPasses => _totalPasses;
    public int HomeTeamPossession => (int) Math.Round(_homeTeamPasses / (double)_totalPasses * 100);
    public int AwayTeamPossession => (int) Math.Round(_awayTeamPasses / (double)_totalPasses * 100);

    private readonly object homeTeamLock = new();
    private readonly object awayTeamLock = new();

    public void SetHomeTeamPasses(int homeTeamPasses)
    {
        lock (homeTeamLock)
        {
            _homeTeamPasses = homeTeamPasses;

            _totalPasses++;
            _lastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void SetAwayTeamPasses(int awayTeamPasses)
    {
        lock (awayTeamLock)
        {
            _awayTeamPasses = awayTeamPasses;

            _totalPasses++;
            _lastUpdatedAt = DateTime.UtcNow;
        }
    }

    public void ShowPossession()
    {
        Console.WriteLine($"\n{_lastUpdatedAt:HH:mm:ss}     {HomeTeamPossession}% : {AwayTeamPossession}%\n");
    }
}