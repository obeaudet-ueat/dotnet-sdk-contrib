using System;
using System.Collections.Generic;
using OpenFeature.Contrib.Providers.Unleash.Resolver;
using Unleash;
using Xunit;

namespace OpenFeature.Contrib.Providers.Unleash.Test.Resolver;

public class DefaultResolverTest
{
    [Fact]
    public void DefaultResolver_Should_Be_Created_Successfully()
    {
        var unleashSettings = new UnleashSettings()
        {
            AppName = "my-app",
            UnleashApi = new Uri("http://localhost:4242"),
            CustomHttpHeaders = new Dictionary<string, string>()
            {
                {"Authorization","<your-api-token>" }
            }
        };

        var unleashProviderConfig = new UnleashProviderConfig(unleashSettings);

        // Act
        var resolver = new DefaultResolver(unleashProviderConfig);

        // Assert
        Assert.NotNull(resolver);
    }
}
