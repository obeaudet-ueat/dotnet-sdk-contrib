# Unleash .NET Provider

The Unleash provider allows you to connect to your Unleash instance.

# .Net SDK usage

## Requirements

- open-feature/dotnet-sdk v2.0.0 > v3.0.0

## Install dependencies

We will first install the **OpenFeature SDK** and the **Unleash provider**.

### .NET Cli

```shell
dotnet add package OpenFeature.Contrib.Providers.Unleash
```

### Package Manager

```shell
NuGet\Install-Package OpenFeature.Contrib.Providers.Unleash
```

### Package Reference

```xml
<PackageReference Include="OpenFeature.Contrib.Providers.Unleash" />
```

### Packet cli

```shell
paket add OpenFeature.Contrib.Providers.Unleash
```

### Cake

```shell
// Install OpenFeature.Contrib.Providers.Unleash as a Cake Addin
#addin nuget:?package=OpenFeature.Contrib.Providers.Unleash

// Install OpenFeature.Contrib.Providers.Unleash as a Cake Tool
#tool nuget:?package=OpenFeature.Contrib.Providers.Unleash
```

## Using the Unleash Provider with the OpenFeature SDK

This example assumes that the Unleash server is running locally

Follow Unleash official documentation to setup local instance https://docs.getunleash.io/deploy/getting-started


When the Unleash service is running, you can use the SDK with the Unleash Provider as in the following example console application:

```csharp
using OpenFeature.Contrib.Providers.Unleash;

namespace OpenFeatureTestApp
{
    class Hello {
        static void Main(string[] args) {

            var unleashSettings = new UnleashSettings()
            {
                AppName = "my-app",
                UnleashApi = new Uri("http://localhost:4242"),
                CustomHttpHeaders = new Dictionary<string, string>()
                {
                  {"Authorization","<your-api-token>" }
                }
            };

            var config = new UnleashProviderConfig(unleashSettings);

            var unleashProvider = new UnleashProvider(config);

            // Set the UnleashProvider as the provider for the OpenFeature SDK
            OpenFeature.Api.Instance.SetProvider(unleashProvider);

            var client = OpenFeature.Api.Instance.GetClient("my-app");

            var val = client.GetBooleanValueAsync("myBoolFlag", false, null);

            // Print the value of the 'myBoolFlag' feature flag
            System.Console.WriteLine(val.Result.ToString());
        }
    }
}
```

