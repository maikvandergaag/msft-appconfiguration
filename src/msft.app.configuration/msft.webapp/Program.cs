using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//connecting to App Configuration with EndPoint and Managed Identity
var endpoint = app.Configuration["AppConfig:Endpoint"];
builder.Configuration.AddAzureAppConfiguration(options => {
    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
            .ConfigureKeyVault(kv => {
                kv.SetCredential(new DefaultAzureCredential());
            })
            .Select("DemoApp:*", LabelFilter.Null)
            .Select("DemoApp:*", app.Environment.EnvironmentName)
            .ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("DemoApp:Sentinel", app.Environment.EnvironmentName , refreshAll: true));
    
    options.UseFeatureFlags(featureFlagOptions => {
        featureFlagOptions.Select("DemoApp-*", app.Environment.EnvironmentName);
        featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(30);
    });
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAzureAppConfiguration();

app.UseAuthorization();
app.MapRazorPages();

app.Run();
