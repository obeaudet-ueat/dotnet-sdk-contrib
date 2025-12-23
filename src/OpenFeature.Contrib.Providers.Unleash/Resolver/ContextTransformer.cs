using System;
using System.Collections.Generic;
using OpenFeature.Model;
using Unleash;

namespace OpenFeature.Contrib.Providers.Unleash.Resolver;

/// <summary>
/// Transformer from OpenFeature context to Unleash context.
/// </summary>
internal static class ContextTransformer
{
    private const string ContextAppName = "appName";
    private const string ContextUserId = "userId";
    private const string ContextEnvironment = "environment";
    private const string ContextRemoteAddress = "remoteAddress";
    private const string ContextSessionId = "sessionId";
    private const string ContextCurrentTime = "currentTime";

    /// <summary>
    /// Transform OpenFeature EvaluationContext to Unleash UnleashContext.
    /// </summary>
    /// <param name="ctx">EvaluationContext</param>
    /// <returns>Unleash UnleashContext</returns>
    internal static UnleashContext Transform(EvaluationContext ctx)
    {
        string appName = null;
        string environment = null;
        string userId = null;
        string sessionId = null;
        string remoteAddress = null;
        DateTime? currentTime = null;
        var properties = new Dictionary<string, string>();

        foreach (var kvp in ctx.AsDictionary())
        {
            var key = kvp.Key;
            var value = kvp.Value;

            switch (key)
            {
                case ContextAppName:
                    appName = value.AsString;
                    break;
                case ContextUserId:
                    userId = value.AsString;
                    break;
                case ContextEnvironment:
                    environment = value.AsString;
                    break;
                case ContextRemoteAddress:
                    remoteAddress = value.AsString;
                    break;
                case ContextSessionId:
                    sessionId = value.AsString;
                    break;
                case ContextCurrentTime:
                    if (DateTime.TryParse(value.AsString, out var dateTime))
                    {
                        currentTime = dateTime;
                    }
                    break;
                default:
                    properties[key] = value.AsString ?? string.Empty;
                    break;
            }
        }

        return new UnleashContext(
            appName: appName,
            environment: environment,
            userId: userId,
            sessionId: sessionId,
            remoteAddress: remoteAddress,
            currentTime: currentTime,
            properties: properties
        );
    }
}
