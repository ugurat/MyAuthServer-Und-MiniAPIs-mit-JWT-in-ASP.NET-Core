using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {


        // http://localhost:5020/api/Manager

        //[Authorize]
        [Authorize(Roles = "admin")]
        //[Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        [HttpGet]
        public IActionResult GetInfo()
        {

            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;

            // Rolle(n) auslesen und verkettet darstellen
            var userRoles = string.Join(", ", User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value));

            //return Ok(new { UserName = userName });
            //return Ok($" Community => UserName: {userName}; UserId: {userId}");
            return Ok($"Manager => UserName: {userName}; UserId: {userId}; Roles: {userRoles}");

        }

    }
}
