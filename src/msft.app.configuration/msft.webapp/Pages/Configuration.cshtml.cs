using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace msft.webapp.Pages {
    public class ConfigurationModel : PageModel {
        private readonly ILogger<ConfigurationModel> _logger;

        public string configurationMessage { get; set; }

        public IConfiguration _configuration { get; }

        public ConfigurationModel(IConfiguration configuration, ILogger<ConfigurationModel> logger) {
            _logger = logger;
            _configuration = configuration;
        }
        
        public void OnGet() {

            configurationMessage = _configuration["DemoApp:Message"];

        }
    }
}