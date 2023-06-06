namespace KafkaStreams.Shared.Models;

public sealed record EnrichedPossession(Guid PlayerId, string PlayerName, short Shirt, Guid TeamId, string TeamName);