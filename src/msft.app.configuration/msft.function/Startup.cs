using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp {
    class Startup : FunctionsStartup {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder) {

            string endpoint = Environment.GetEnvironmentVariable("Endpoint");
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options => {
                options.Connect(new Uri(endpoint), new DefaultAzureCredential())
                        .Select("DemoFunc:*", LabelFilter.Null)
                        .Select("DemoFunc:*", "Demo")
                        .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("DemoFunc:Sentinel", "Demo", refreshAll: true));

                options.UseFeatureFlags(featureFlagOptions => {
                    featureFlagOptions.Select("DemoFunc-*");
                    featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(30);
                });
            });
        }

        public override void Configure(IFunctionsHostBuilder builder) {
        }
    }
}