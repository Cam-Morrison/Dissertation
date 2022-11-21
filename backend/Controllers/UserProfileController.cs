using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using backend.entity;
using backend.model;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ILogger<UserProfileController> _logger;
        private readonly IFeatureFlagService _featureFlag;
        private IUserService _userService;
        private  IMarketDataService _marketDataService;
        private readonly IConfiguration _configuration;


        public UserProfileController
        (
            ILogger<UserProfileController> logger, 
            IFeatureFlagService featureFlag,
            IUserService userService,
            IConfiguration configuration,
            IMarketDataService marketDataService
        )
        {
            _logger = logger;
            _featureFlag = featureFlag;
            _userService = userService;
            _marketDataService = marketDataService;
            _configuration = configuration;
        }

        [HttpPost]      
        [Route("/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Used to login to system.")]
        public async Task<IActionResult> Login(Login input)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("postlogin"))
                {
                    var response = await _userService.Login(input);
                    if(response == "Username invalid." || response == "Password invalid.") 
                    {
                        return Unauthorized(response);
                    }
                    else 
                    {
                        return Ok(response);
                    }
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.Login(Login input)");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]      
        [Route("/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Creates account in the system using username, password and confirmation.")]
        public async Task<IActionResult> Register(Register input)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("postRegistration"))
                {
                    var response = await _userService.Register(input);
                    if(response == "User already exists.") 
                    {
                        return BadRequest(response);
                    }
                    else 
                    {
                        return Ok(response);
                    }
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.Register(Register input)");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]   
        [Route("/addToWatchlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Adds a stock to a user's watchlist")]
        public async Task<IActionResult> AddToWatchlist(string username, string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("postRegistration"))
                {
                   var resp = _userService.AddToWatchlist(username, ticker);
                   if(resp == "Unauthorized") {
                        return Unauthorized();
                   } 
                   return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.AddToWatchlist(String ticker)");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}