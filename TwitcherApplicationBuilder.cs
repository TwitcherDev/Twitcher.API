using Microsoft.Extensions.Logging;

namespace Twitcher.API;

/// <summary>Builder for <see cref="TwitcherApplication"/></summary>
public class TwitcherApplicationBuilder
{
    private readonly string _clientId;
    private readonly string _clientSecret;

    private ILoggerFactory? _loggerFactory;
    private ILogger? _logger;

    private StateCollection? _states;
    private TwitcherAPICollection? _collection;

    /// <summary>Create an instance of <see cref="TwitcherApplicationBuilder"/></summary>
    /// <param name="clientId">Id of the application</param>
    /// <param name="clientSecret">Secret of the application</param>
    public TwitcherApplicationBuilder(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    /// <summary>Creates new <see cref="TwitcherApplication"/> instance</summary>
    /// <returns>Created <see cref="TwitcherApplication"/> instance</returns>
    public TwitcherApplication Build()
    {
        return new TwitcherApplication(_clientId, _clientSecret, _states, _collection, _loggerFactory, _logger);
    }

    /// <summary>Add states for secure code authorization</summary>
    /// <param name="stateLiveTime">Lifetime of every state, default: 24 hours</param>
    /// <param name="statesLimit">Maximum number of unused states, default: 1000</param>
    /// <param name="stateLength">Length of states, dafault: 32</param>
    public TwitcherApplicationBuilder UseAuthorizeStates(TimeSpan? stateLiveTime = default, int statesLimit = 1000, int stateLength = 32)
    {
        if (_states != null)
            throw new NotSupportedException($"{nameof(StateCollection)} already in use");

        _states = new StateCollection(stateLiveTime ?? TimeSpan.FromHours(24), statesLimit, stateLength);
        return this;
    }

    /// <summary>Add Collection to manage <see cref="TwitcherAPI"/> instances in the application for constant access without creating unnecessary instances</summary>
    public TwitcherApplicationBuilder UseAPICollection()
    {
        if (_collection != null)
            throw new NotSupportedException($"{nameof(TwitcherAPICollection)} already in use");

        _collection = new TwitcherAPICollection(_clientId, _clientSecret, _loggerFactory);
        return this;
    }

    /// <param name="loggerFactory"><see cref="ILoggerFactory"/> for create loggers for logging</param>
    public TwitcherApplicationBuilder UseLoggerFactory(ILoggerFactory? loggerFactory)
    {
        _loggerFactory = loggerFactory;
        return this;
    }

    /// <param name="logger"><see cref="ILogger"/> for logging</param>
    public TwitcherApplicationBuilder UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }
}
