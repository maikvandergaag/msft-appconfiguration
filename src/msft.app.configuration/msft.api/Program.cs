using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to web app for application configuration.
builder.Services.AddRazorPages();
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//connecting to App Configuration with EndPoint and Managed Identity
var endpoint = app.Configuration["AppConfig:Endpoint"];
builder.Configuration.AddAzureAppConfiguration(options => {
    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
            .ConfigureKeyVault(kv => {
                kv.SetCredential(new DefaultAzureCredential());
            })
            .UseFeatureFlags()
            .Select("Demo:*", LabelFilter.Null)
            .ConfigureRefresh(refreshOptions =>
                refreshOptions.Register("Demo:Config:Sentinel", refreshAll: true));
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAzureAppConfiguration();

app.MapControllers();

app.Run();
