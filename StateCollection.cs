﻿namespace Twitcher.API;

/// <summary>Managing states for secure code authorization</summary>
public class StateCollection
{
    private readonly TimeSpan _stateLiveTime;
    private readonly int _stateCountLimit;
    private readonly int _stateLength;

    private readonly List<(string state, DateTime expiresIn)> _states;

    /// <summary>Create an instance of <see cref="StateCollection"/></summary>
    /// <param name="stateLiveTime">Lifetime of every state</param>
    /// <param name="stateCountLimit">Maximum number of unused states</param>
    /// <param name="stateLength">Length of states</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public StateCollection(TimeSpan stateLiveTime, int stateCountLimit, int stateLength)
    {
        if (stateCountLimit < 1)
            throw new ArgumentOutOfRangeException(nameof(stateCountLimit));
        if (stateLength < 1)
            throw new ArgumentOutOfRangeException(nameof(stateLength));
        _stateLiveTime = stateLiveTime;
        _stateCountLimit = stateCountLimit;
        _stateLength = stateLength;

        _states = new List<(string state, DateTime createdAt)>();
    }

    /// <summary>Creates a new random state</summary>
    /// <returns>State string</returns>
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

    /// <summary>Checks if the state was generated by this instance and if it is overdue</summary>
    /// <param name="state">State for check</param>
    /// <returns><see langword="true"/> if the state is correct, otherwise <see langword="false"/></returns>
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
