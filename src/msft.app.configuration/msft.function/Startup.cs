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
                        .ConfigureKeyVault(kv => {
                            kv.SetCredential(new DefaultAzureCredential());
                        })
                        .UseFeatureFlags()
                        .Select("Demo:FunctionApp:*", LabelFilter.Null)
                        .Select("Demo:FunctionApp:*", "Demo")
                        .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("Demo:Config:Sentinel", refreshAll: true));
            });
        }

        public override void Configure(IFunctionsHostBuilder builder) {
        }
    }
}