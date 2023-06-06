using KafkaFlow.TypedHandler;

namespace KafkaStreams.Consumer.Handlers;

public sealed class TeamPossessionHandler : IMessageHandler<PossessionChangedEvent>
{
    private readonly State _state;

    public TeamPossessionHandler(State state) =>
        _state = state;

    public Task Handle(IMessageContext context, PossessionChangedEvent message)
    {
        if (message.TeamName == Constants.HomeTeam)
        {
            _state.SetHomeTeamPasses(message.TeamTotalPasses);
        }
        else
        {
            _state.SetAwayTeamPasses(message.TeamTotalPasses);
        }

        _state.ShowPossession();

        return Task.CompletedTask;
    }
}