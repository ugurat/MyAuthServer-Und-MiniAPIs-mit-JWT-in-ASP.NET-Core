using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi5.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        // GET http://localhost:5050/api/Device

        [Authorize] // TokenByClient
        //[Authorize(Roles = "admin")]
        //[Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        //[Authorize(Policy = "CityPolicy")]
        [HttpGet]
        public IActionResult GetInfo()
        {
            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;

            //return Ok(new { UserName = userName });
            //return Ok($" Community => UserName: {userName}; UserId: {userId}");
            return Ok($"Device => Gerätezugriff gewährt");

        }

    }
}
