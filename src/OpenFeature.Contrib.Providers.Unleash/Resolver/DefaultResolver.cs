using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenFeature.Model;
using Unleash;

namespace OpenFeature.Contrib.Providers.Unleash.Resolver;

internal class DefaultResolver : IResolver
{
    private readonly UnleashProviderConfig _config;
    private IUnleash _unleash;
    private bool _isInitialized;

    internal DefaultResolver(UnleashProviderConfig config)
    {
        _config = config;
    }

    private void IsInitialized()
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException("Unleash Provider already initialized");
        }
    }

    public async Task Init()
    {
        _isInitialized = true;
        _unleash = await _config.UnleashFactory.CreateClientAsync(_config.UnleashSettings, synchronousInitialization: true);
    }

    public Task Shutdown()
    {
        _unleash?.Dispose();
        _unleash = null;
        return Task.CompletedTask;
    }

    public Task<ResolutionDetails<bool>> ResolveBooleanValueAsync(string flagKey, bool defaultValue, EvaluationContext context = null)
    {
        var unleashContext = context == null ? new UnleashContext() : ContextTransformer.Transform(context);
        var value = _unleash.IsEnabled(flagKey, unleashContext, defaultValue);

        return Task.FromResult(new ResolutionDetails<bool>(
            flagKey,
            value
        ));
    }

    public async Task<ResolutionDetails<string>> ResolveStringValueAsync(string flagKey, string defaultValue, EvaluationContext context = null)
    {
        var valueResult = await ResolveStructureValueAsync(flagKey, new Value(defaultValue), context);

        return new ResolutionDetails<string>(
            flagKey,
            valueResult.Value?.AsString ?? defaultValue,
            variant: valueResult.Variant,
            reason: valueResult.Reason,
            errorType: valueResult.ErrorType,
            errorMessage: valueResult.ErrorMessage,
            flagMetadata: valueResult.FlagMetadata
        );
    }

    public async Task<ResolutionDetails<int>> ResolveIntegerValueAsync(string flagKey, int defaultValue, EvaluationContext context = null)
    {
        var valueResult = await ResolveStructureValueAsync(flagKey, new Value(defaultValue), context);

        var value = GetIntegerValue(valueResult, defaultValue);

        return new ResolutionDetails<int>(
            flagKey,
            value,
            variant: valueResult.Variant,
            reason: valueResult.Reason,
            errorType: valueResult.ErrorType,
            errorMessage: valueResult.ErrorMessage,
            flagMetadata: valueResult.FlagMetadata
        );
    }

    private static int GetIntegerValue(ResolutionDetails<Value> valueResult, int defaultValue)
    {
        if (valueResult.Value == null) return defaultValue;

        var valueStr = valueResult.Value.AsString;
        return int.TryParse(valueStr, out var intValue) ? intValue : defaultValue;
    }

    public async Task<ResolutionDetails<double>> ResolveDoubleValueAsync(string flagKey, double defaultValue, EvaluationContext context = null)
    {
        var valueResult = await ResolveStructureValueAsync(flagKey, new Value(defaultValue), context);

        var value = GetDoubleValue(valueResult, defaultValue);

        return new ResolutionDetails<double>(
            flagKey,
            value,
            variant: valueResult.Variant,
            reason: valueResult.Reason,
            errorType: valueResult.ErrorType,
            errorMessage: valueResult.ErrorMessage,
            flagMetadata: valueResult.FlagMetadata
        );
    }

    private static double GetDoubleValue(ResolutionDetails<Value> valueResult, double defaultValue)
    {
        if (valueResult.Value == null) return defaultValue;

        var valueStr = valueResult.Value.AsString;
        if (double.TryParse(valueStr, out var doubleValue))
        {
            return doubleValue;
        }
        return defaultValue;
    }

    public async Task<ResolutionDetails<Value>> ResolveStructureValueAsync(string flagKey, Value defaultValue, EvaluationContext context = null)
    {
        var unleashContext = context == null ? new UnleashContext() : ContextTransformer.Transform(context);
        var variant = _unleash.GetVariant(flagKey, unleashContext);

        string variantName = null;
        Value value = null;
        var flagMetadata = new Dictionary<string, object>();

        if (variant.Enabled)
        {
            variantName = variant.Name;
            if (variant.Payload != null && !string.IsNullOrEmpty(variant.Payload.Value))
            {
                value = new Value(variant.Payload.Value);
                flagMetadata["payload-type"] = variant.Payload.Type;
            }
        }
        else
        {
            value = defaultValue;
        }

        flagMetadata["enabled"] = variant.Enabled;

        return await Task.FromResult(new ResolutionDetails<Value>(
            flagKey,
            value ?? defaultValue,
            variant: variantName,
            flagMetadata: new ImmutableMetadata(flagMetadata)
        ));
    }
}
