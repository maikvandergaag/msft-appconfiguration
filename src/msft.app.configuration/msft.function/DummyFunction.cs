using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace msft.function
{
    public class DummyFunction
    {
        private readonly IConfiguration _configuration;

        public DummyFunction(IConfiguration configuration) {
            _configuration = configuration;
        }

        [FunctionName("DummyFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string configKey = "Demo:FunctionApp:Message";
            string message = _configuration[configKey];
            
            log.LogInformation($"Found the config in Azure App Configuration {message}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string responseMessage = string.IsNullOrEmpty(message)
                ? "There is no configuration value with the key 'Demo:FunctionApp:Message' in Azure App Configuration"
                : message;

            return new OkObjectResult(responseMessage);
        }
    }
}
