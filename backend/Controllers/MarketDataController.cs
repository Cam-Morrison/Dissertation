using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
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
        [Route("/tickers/search/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Searches for any stocks using {ticker} which is the user entered value.")]
        public async Task<IActionResult> SearchForStock(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
                {
                    return Ok(_marketDataService.SearchForStock(ticker.ToUpper()));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.SearchForStock()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
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
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
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

        [HttpGet]
        [Route("/detailsPageContent/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = 
        "Gets the historical prices of {ticker}, the A.I. predicted price and stock key metrics. All of which are used to populate the stock details page.")]
        public async Task<IActionResult> GetDetailsPageContent(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
                {
                    return Ok(_marketDataService.GetDetailsPageContent(ticker));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetStockHistory()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("/movers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Gets the most active stocks")]
        public async Task<IActionResult> GetActiveStocks()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
                {
                    return Ok(_marketDataService.GetActiveStocks(false));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetActiveStocks()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("/timeSeriesForecasting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Attempts to predict future market value using time series forecasting. This is only for stock details page.")]
        public async Task<IActionResult> GetStockPrediction()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("stockPriceFunctionality"))
                {
                    return Ok(_marketDataService.GetPricePrediction());
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetStockPrediction()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}