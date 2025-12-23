using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenFeature.Contrib.Providers.Unleash.Resolver;
using OpenFeature.Model;
using Unleash.Events;
using Unleash.Internal;

namespace OpenFeature.Contrib.Providers.Unleash;

/// <summary>
/// Provider implementation for Unleash.
/// </summary>
public class UnleashProvider : FeatureProvider
{
    private readonly ILogger<UnleashProvider> _logger;
    private readonly UnleashProviderConfig _config;
    private readonly IResolver _resolver;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="config">UnleashProviderConfig</param>
    public UnleashProvider(UnleashProviderConfig config)
        : this(config, Microsoft.Extensions.Logging.Abstractions.NullLogger<UnleashProvider>.Instance)
    {
    }


    /// <summary>
    /// Constructor with logger
    /// </summary>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UnleashProvider(UnleashProviderConfig config, ILogger<UnleashProvider> logger)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _resolver = new DefaultResolver(_config);
    }

    /// <inheritdoc />
    public override Metadata GetMetadata()
    {
        return new Metadata("Unleash Provider");
    }

    /// <inheritdoc />
    public override async Task InitializeAsync(EvaluationContext context, CancellationToken cancellationToken = default)
    {
        await _resolver.Init();

        // if (_isInitialized)
        // {
        //     throw new InvalidOperationException("Provider already initialized");
        // }
        //
        // _isInitialized = true;
        //
        // // Initialize Unleash
        // var unleashFactory = _config.UnleashFactory ?? new DefaultUnleashFactory();
        // _unleash = unleashFactory.CreateUnleash(_config.UnleashSettings);
        //
        // // Configure event callbacks to emit provider events
        // _unleash.ConfigureEvents(cfg =>
        // {
        //     cfg.TogglesUpdatedEvent += OnTogglesUpdated;
        //     cfg.ErrorEvent += OnError;
        // });
        //
        // _logger.LogInformation("Finished initializing provider");
        //
        // await Task.CompletedTask;
    }

    /// <inheritdoc />
    public override Task<ResolutionDetails<bool>> ResolveBooleanValueAsync(string flagKey, bool defaultValue, EvaluationContext? context = null, CancellationToken cancellationToken = default)
    {
        return _resolver.ResolveBooleanValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<string>> ResolveStringValueAsync(string flagKey, string defaultValue, EvaluationContext? context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveStringValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<int>> ResolveIntegerValueAsync(string flagKey, int defaultValue, EvaluationContext? context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveIntegerValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override async Task<ResolutionDetails<double>> ResolveDoubleValueAsync(string flagKey, double defaultValue, EvaluationContext? context = null, CancellationToken cancellationToken = default)
    {
        return await _resolver.ResolveDoubleValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override Task<ResolutionDetails<Value>> ResolveStructureValueAsync(string flagKey, Value defaultValue, EvaluationContext? context = null, CancellationToken cancellationToken = default)
    {
        return _resolver.ResolveStructureValueAsync(flagKey, defaultValue, context);
    }

    /// <inheritdoc />
    public override async Task ShutdownAsync(CancellationToken cancellationToken = default)
    {
        await _resolver.Shutdown();
    }

    private void OnTogglesUpdated(TogglesUpdatedEvent evt)
    {
        _logger.LogInformation("Toggles updated at {UpdatedOn}", evt.UpdatedOn);
        // Configuration changed - providers can implement event emission if needed
    }

    private void OnError(ErrorEvent evt)
    {
        _logger.LogError(evt.Error, "Unleash error: {ErrorType}, Resource: {Resource}", evt.ErrorType, evt.Resource);
        // Not emitting provider error, since some unleash errors do not require
        // changing provider state to error
    }
}
