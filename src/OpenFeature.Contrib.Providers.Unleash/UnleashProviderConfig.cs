using System;
using Unleash;
using Unleash.ClientFactory;

namespace OpenFeature.Contrib.Providers.Unleash;

/// <summary>
/// Options for initializing Unleash provider.
/// </summary>
public class UnleashProviderConfig
{
    /// <summary>
    ///
    /// </summary>
    public UnleashSettings UnleashSettings { get; }

    /// <summary>
    ///
    /// </summary>
    public IUnleashClientFactory UnleashFactory { get; }

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
        UnleashSettings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        UnleashFactory = factory ?? throw new System.ArgumentNullException(nameof(factory));
    }
}
