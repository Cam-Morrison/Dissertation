using Microsoft.AspNetCore.Mvc;
using backend.services;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using backend.entity;
using backend.model;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ILogger<UserProfileController> _logger;
        private readonly IFeatureFlagService _featureFlag;
        private IUserService _userService;

        public UserProfileController
        (
            ILogger<UserProfileController> logger, 
            IFeatureFlagService featureFlag,
            IUserService userService
        )
        {
            _logger = logger;
            _featureFlag = featureFlag;
            _userService = userService;
        }

        [HttpPost]      
        [Route("/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Used to login to system.")]
        public async Task<IActionResult> Login(Login input)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("getNewsSentiment"))
                {
                    if(await _userService.Login(input) == false) 
                    {
                        return BadRequest("Username or password is incorrect.");
                    }
                    else 
                    {
                        return Ok("Logged in successfully.");
                    }
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.Login()");
                return StatusCode(StatusCodes.Status401Unauthorized);
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
                if(await _featureFlag.GetFeatureFlagAsync("getNewsSentiment"))
                {
                    if(await _userService.Register(input) == false) 
                    {
                        return BadRequest("User already exists.");
                    }
                    else 
                    {
                        return Ok("Registered");
                    }
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("UserProfileController.Register()");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}