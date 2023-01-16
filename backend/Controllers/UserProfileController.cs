using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using backend.entity;
using backend.model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpPost, AllowAnonymous]      
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
                    if(response == "Username invalid." || response == "Password invalid." || response == "Account locked by admin.") 
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

        [HttpPost, AllowAnonymous]      
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

        [HttpGet] 
        [Route("/toggleAIpreference")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Toggles A.I. assist feature (on/off) for signed in user and returns updated JWT.")]
        public async Task<IActionResult> toggleAIpreference()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("postlogin"))
                {
                    var resp = _userService.toggleAIpreference(HttpContext.User.Identity.Name);
                    return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.toggleAIpreference()");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet] 
        [Route("/logSignOut")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Adds a log of a user's logged out action to tasks table for administrators to view.")]
        public async Task<IActionResult> LogSignOut()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("adminFunctionality"))
                {
                    var resp = _userService.LogSignOut(HttpContext.User.Identity.Name);
                    return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.LogSignOut()");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet] 
        [Route("/addToWatchlist/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Adds a stock to a user's watchlist")]
        public async Task<IActionResult> AddToWatchlist(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("watchlistFeature"))
                {
                    var resp = _userService.AddToWatchlist(HttpContext.User.Identity.Name, ticker.ToUpper());
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

        [HttpGet]   
        [Route("/getWatchlist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Gets a list of watchlist stocks.")]
        public async Task<IActionResult> GetWatchlist()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("watchlistFeature"))
                {
                    var resp = _userService.GetWatchlist(HttpContext.User.Identity.Name);
                    return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.GetWatchlist()");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]   
        [Route("/removeFromWatchlist/{ticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Remove ticker from watch list.")]
        public async Task<IActionResult> RemoveFromWatchlist(string ticker)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("watchlistFeature"))
                {
                    var resp = _userService.RemoveFromWatchList(HttpContext.User.Identity.Name, ticker.ToUpper());
                    return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.RemoveFromWatchList(string ticker)");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]   
        [Route("/updateWatchlistTitle/{newTitle}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Updates the title of user's watchlist.")]
        public async Task<IActionResult> UpdateWatchlistTitle(string newTitle)
        {
            try 
            {
                if(await _featureFlag.GetFeatureFlagAsync("watchlistFeature"))
                {
                    var resp = _userService.UpdateWatchListTitle(HttpContext.User.Identity.Name, newTitle);
                    return Ok(resp);
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.UpdateWatchListTitle(string newTitle)");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}