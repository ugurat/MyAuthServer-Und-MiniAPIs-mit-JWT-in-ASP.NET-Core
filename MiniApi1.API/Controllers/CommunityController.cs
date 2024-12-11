using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {

        // http://localhost:5010/api/Community

        [Authorize] // Token (über Email, Password)
        //[Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        [HttpGet]
        public IActionResult GetInfo()
        {

            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;

            //return Ok(new { UserName = userName });
            return Ok($" Community => UserName: {userName}; UserId: {userId}");
        }


    }
}
