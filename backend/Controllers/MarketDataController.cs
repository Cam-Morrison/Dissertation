using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet]      
        [Route("/price/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Gets the current price of {ticker}")]
        public async Task<IActionResult> GetStockPrice(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getMarketMovement"))
                {
                    return Ok(_marketDataService.GetStockPrice(ticker));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetStockPrice()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet]
        [Route("/details/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Gets the details of {ticker} for specific stock page")]
        public async Task<IActionResult> GetStockDetail(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getMarketMovement"))
                {
                    return Ok(_marketDataService.GetStockDetail(ticker));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetStockDetail()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}