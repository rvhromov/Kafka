namespace KafkaStreams.Shared;

public static class Constants
{
    public const string BootstrapServers = "localhost:9092";

    public const string LineupTopic = "Lineup";
    public const string PassesTopic = "Passes";

    public const string TeamPossessionTopic = "TeamPossession";

    public static readonly Guid HomeTeamId = new("73468afb-911a-4dde-814f-0157961f59db");
    public static readonly Guid AwayTeamId = new("4baefeb8-a38d-48a4-86c0-0cd2dcf0c351");
    public const string HomeTeam = "Real Madrid";
    public const string AwayTeam = "Barcelona";
}