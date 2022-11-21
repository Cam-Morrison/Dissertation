using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketNewsController : ControllerBase
    {
        private readonly ILogger<MarketNewsController> _logger;
        private readonly IFeatureFlagService _featureFlag;
        private INewsService _newsService;

        public MarketNewsController
        (
            ILogger<MarketNewsController> logger, 
            IFeatureFlagService featureFlag,
            INewsService newsService
        )
        {
            _logger = logger;
            _featureFlag = featureFlag;
            _newsService = newsService;
        }

        [HttpGet]      
        [Route("/news/daily")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Returns a list of the daily news.")]
        public async Task<IActionResult> GetDailyNews()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getNewsSentiment"))
                {
                    return Ok(_newsService.GetDailyNews());
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketNewsController.GetDailyNews()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]      
        [Route("/sentiment/{text}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Gets the sentiment analysis classification and score of text")]
        public async Task<IActionResult> GetNewsSentiment(string text)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getNewsSentiment"))
                {
                    return Ok(_newsService.GetSentiment(text));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketNewsController.GetNewsSentiment()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}