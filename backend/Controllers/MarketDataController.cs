using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly ILogger<MarketDataController> _logger;
        private readonly IFeatureFlagService _featureFlag;
        private IMarketDataService _marketDataService;

        public MarketDataController
        (
            ILogger<MarketDataController> logger, 
            IFeatureFlagService featureFlag,
            IMarketDataService marketDataService
        )
        {
            _logger = logger;
            _featureFlag = featureFlag;
            _marketDataService = marketDataService;
        }

        [HttpGet(Name = "GetStockPrice")]
        public async Task<IActionResult> Get(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getMarketMovement"))
                {
                    return Ok(_marketDataService.GetMarketData());
                    // return Ok(_marketDataService.GetStockPriceAsync(ticker));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("WeatherForecastController.Get()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}