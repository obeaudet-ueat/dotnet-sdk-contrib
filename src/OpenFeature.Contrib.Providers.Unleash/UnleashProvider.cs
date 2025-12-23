using System;
using System.Threading;
using System.Threading.Tasks;
using OpenFeature.Contrib.Providers.Unleash.Resolver;
using OpenFeature.Model;

namespace OpenFeature.Contrib.Providers.Unleash;

/// <summary>
/// Provider implementation for Unleash.
/// </summary>
public class UnleashProvider : FeatureProvider
{
    private readonly IResolver _resolver;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="unleashProviderConfig"></param>
    public UnleashProvider(UnleashProviderConfig unleashProviderConfig)
    {
        var config = unleashProviderConfig ?? throw new ArgumentNullException(nameof(unleashProviderConfig));
        _resolver = new DefaultResolver(config);
    }

    /// <inheritdoc />
    public override Metadata GetMetadata()
    {
        return new Metadata("Unleash Provider");
    }

    /// <inheritdoc />
    public override async Task InitializeAsync(EvaluationContext context, CancellationToken cancellationToken = default)
    {
        await _resolver.Init().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override Task<ResolutionDetails<bool>> ResolveBooleanValueAsync(string flagKey, bool defaultValue, EvaluationContext context = null, CancellationToken cancellationToken = default)
    {
        return _resolver.ResolveBooleanValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<string>> ResolveStringValueAsync(string flagKey, string defaultValue, EvaluationContext context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveStringValueAsync(flagKey, defaultValue, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<int>> ResolveIntegerValueAsync(string flagKey, int defaultValue, EvaluationContext context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveIntegerValueAsync(flagKey, defaultValue, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<double>> ResolveDoubleValueAsync(string flagKey, double defaultValue, EvaluationContext context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveDoubleValueAsync(flagKey, defaultValue, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<Value>> ResolveStructureValueAsync(string flagKey, Value defaultValue, EvaluationContext context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveStructureValueAsync(flagKey, defaultValue, context).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public override async Task ShutdownAsync(CancellationToken cancellationToken = default)
    {
        await _resolver.Shutdown().ConfigureAwait(false);
    }
}
