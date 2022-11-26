using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

// Add services to web app for application configuration.
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var endpoint = app.Configuration["AppConfig:Endpoint"];
builder.Configuration.AddAzureAppConfiguration(options => {
    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
    .ConfigureKeyVault(kv => {
        kv.SetCredential(new DefaultAzureCredential());
    })
            .Select("DemoApi:*", LabelFilter.Null)
            .Select("DemoApi:*", app.Environment.EnvironmentName)
            .ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("DemoApi:Sentinel", app.Environment.EnvironmentName, refreshAll: true));

    options.UseFeatureFlags(featureFlagOptions => {
        featureFlagOptions.Select("DemoApi-*", LabelFilter.Null);
        featureFlagOptions.Select("DemoApi-*", app.Environment.EnvironmentName);
        featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(30);
    });
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAzureAppConfiguration();

app.MapControllers();

app.Run();
