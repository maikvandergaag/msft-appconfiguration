using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace msft.webapp.Pages {
    [FeatureGate("DemoApp-Navigation")]
    public class FeatureModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFeatureManager _featureManager;

        public FeatureModel(ILogger<IndexModel> logger, IFeatureManager manager) {
            _logger = logger;
            _featureManager = manager;
        }

        public void OnGet() {

        }
    }
}