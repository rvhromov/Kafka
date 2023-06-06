using KafkaStreams.Processor.SerDes;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;

namespace KafkaStreams.Processor;

public sealed class TopologyBuilder
{
    public static Topology Build()
    {
        var builder = new StreamBuilder();

        IKTable<Guid, PlayerAddedToStartingLineupEvent> lineupTable = builder
            .Table<Guid, PlayerAddedToStartingLineupEvent, GuidSerDes, JsonSerDes<PlayerAddedToStartingLineupEvent>>(Constants.LineupTopic);

        IKStream<Guid, PossessionChangedEvent> possessionStream = builder
            .Stream<Guid, PossessionChangedEvent, GuidSerDes, JsonSerDes<PossessionChangedEvent>>(Constants.PassesTopic);

        IKStream<Guid, EnrichedPossession> enrichedStream = possessionStream
            .Join<PlayerAddedToStartingLineupEvent, EnrichedPossession, JsonSerDes<PlayerAddedToStartingLineupEvent>, JsonSerDes<EnrichedPossession>>(
                lineupTable,
                (left, right) => new EnrichedPossession(left.PlayerId, right.PlayerName, right.Shirt, right.TeamId, right.TeamName));

        var teamPossessions = enrichedStream
            .GroupBy<Guid, GuidSerDes>((key, value) => value.TeamId)
            .Aggregate<PossessionChangedEvent, JsonSerDes<PossessionChangedEvent>>(
                () => new PossessionChangedEvent(),
                (key, value, teamPossession) =>
                {
                    teamPossession.TeamId = value.TeamId;
                    teamPossession.TeamName = value.TeamName;
                    teamPossession.TeamTotalPasses = ++teamPossession.TeamTotalPasses;
                    teamPossession.TimeStamp = DateTime.UtcNow;

                    return teamPossession;
                })
            .ToStream()
            .Peek((k, v) => Console.WriteLine($"Total passes by {v.TeamName}: {v.TeamTotalPasses}"));

        teamPossessions.To<GuidSerDes, JsonSerDes<PossessionChangedEvent>>(Constants.TeamPossessionTopic);

        return builder.Build();
    }
}