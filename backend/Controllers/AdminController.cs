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
    public class AdminController : ControllerBase
    {
        private readonly ILogger<MarketDataController> _logger;
        private readonly IFeatureFlagService _featureFlag;
        private IAdminService _adminService;

        public AdminController
        (
            ILogger<MarketDataController> logger, 
            IFeatureFlagService featureFlag,
            IAdminService adminService
        )
        {
            _logger = logger;
            _featureFlag = featureFlag;
            _adminService = adminService;
        }

        [HttpGet]      
        [Route("/getTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Returns all audit log (task) information for admin page.")]
        public async Task<IActionResult> GetTaskHistory()
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("adminFunctionality"))
                {
                    return Ok(_adminService.getTaskHistory(HttpContext.User.Identity.Name));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetTaskHistory()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]      
        [Route("/toggleAccountLock/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Changes the current boolean value to lock or unlock a user's account from accessing the site.")]
        public async Task<IActionResult> ToggleAccountLock(int userId)
        {
            try
            {
                if(await _featureFlag.GetFeatureFlagAsync("adminFunctionality"))
                {
                    return Ok(_adminService.toggleAccountlock(HttpContext.User.Identity.Name, userId));
                } 
                return Ok("Feature not implemented");
            }
            catch(Exception ex)
            {
                Log.Information("MarketDataController.GetTaskHistory()");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}