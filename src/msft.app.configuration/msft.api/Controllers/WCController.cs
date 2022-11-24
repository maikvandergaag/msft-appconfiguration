using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace msft.api.Controllers {
    [ApiController]
    [Route("[controller]")]
    [FeatureGate("DemoApi-WC")]
    public class WCController : ControllerBase {
        private static readonly string[] Teams = new[]
        {
        "Portugal", "Germany", "Costa Rica", "Spain", "France", "Netherlands", "Brazil", "Mexico", "Argentina", "Belgium", "England", "Colombia", "Uruguay", "Croatia", "Denmark", "Sweden", "Switzerland", "Peru", "Senegal", "Iceland", "Poland", "Serbia", "Tunisia", "Egypt", "Morocco", "Iran", "Nigeria", "Australia", "Japan", "Panama", "Korea Republic", "Saudi Arabia", "Russia", "Costa Rica", "Sweden", "Belgium", "England", "Panama", "Tunisia", "Colombia", "Japan", "Poland", "Senegal", "Uruguay", "Portugal", "Russia", "Egypt", "Saudi Arabia", "Iran", "Spain", "Morocco", "Denmark", "Australia", "France", "Argentina", "Croatia", "Nigeria", "Iceland", "Peru", "Korea Republic", "Germany", "Mexico", "Brazil"
        };

        private readonly ILogger<WCController> _logger;

        private readonly IFeatureManager _featureManager;

        public WCController(ILogger<WCController> logger, IFeatureManager manager) {
            _logger = logger;
            _featureManager = manager;
        }

        [HttpGet(Name = "GetTeams")]
        public IEnumerable<Teams> Get() {

            IEnumerable<Teams> retVal = new List<Teams>();

            if (_featureManager.IsEnabledAsync("DemoApi-Points").Result) {
                retVal = Enumerable.Range(1, 5).Select(index => new Teams {
                    Points = Random.Shared.Next(0, 20),
                    Name = Teams[Random.Shared.Next(Teams.Length)]
                });
            } else {
                retVal = Enumerable.Range(1, 5).Select(index => new Teams {
                    Name = Teams[Random.Shared.Next(Teams.Length)]
                });
            }

            return retVal;
        }
    }
}