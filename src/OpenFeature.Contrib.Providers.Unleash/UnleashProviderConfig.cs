using System;
using Unleash;
using Unleash.ClientFactory;

namespace OpenFeature.Contrib.Providers.Unleash;

/// <summary>
/// Options for initializing Unleash provider.
/// </summary>
public class UnleashProviderConfig
{
    private readonly UnleashSettings _settings;
    private readonly IUnleashClientFactory _factory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings"></param>
    public UnleashProviderConfig(UnleashSettings settings)
        : this(settings, new UnleashClientFactory())
    {
    }

    /// <summary>
    /// Constructor with factory
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="factory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UnleashProviderConfig(UnleashSettings settings, IUnleashClientFactory factory)
    {
        _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        _factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
    }

    /// <summary>
    ///
    /// </summary>
    public UnleashSettings UnleashSettings => _settings;

    /// <summary>
    ///
    /// </summary>
    public IUnleashClientFactory UnleashFactory => _factory;
}
