namespace Twitcher.API;

internal class StateCollection
{
    private readonly TimeSpan _stateLiveTime;
    private readonly int _stateCountLimit;
    private readonly int _stateLength;

    private readonly List<(string state, DateTime expiresIn)> _states;

    public StateCollection(TimeSpan stateLiveTime, int stateCountLimit, int stateLength)
    {
        if (stateCountLimit < 1)
            throw new ArgumentOutOfRangeException(nameof(stateCountLimit));
        _stateLiveTime = stateLiveTime;
        _stateCountLimit = stateCountLimit;
        _stateLength = stateLength;

        _states = new List<(string state, DateTime createdAt)>();
    }

    public string CreateState()
    {
        lock (_states)
        {
            var now = DateTime.UtcNow;
            (string state, DateTime expiresIn)? oldestState = default;
            foreach (var state in _states)
            {
                if (state.expiresIn < DateTime.UtcNow)
                    _states.Remove(state);
                else if (!oldestState.HasValue || oldestState.Value.expiresIn > state.expiresIn)
                    oldestState = state;
            }
            if (oldestState.HasValue && _states.Count == _stateCountLimit)
                _states.Remove(oldestState.Value);
            var newState = (state: GenerateState(_stateLength), expiresIn: now + _stateLiveTime);
            _states.Add(newState);
            return newState.state;
        }
    }

    public bool PassingState(string state)
    {
        lock (_states)
        {
            var stateValue = _states.FirstOrDefault(s => s.state == state);
            _states.Remove(stateValue);
            return stateValue.expiresIn >= DateTime.UtcNow;
        }
    }

    private static string GenerateState(int length) => new(Enumerable.Range(0, length).Select(e => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[Random.Shared.Next(62)]).ToArray());
}
