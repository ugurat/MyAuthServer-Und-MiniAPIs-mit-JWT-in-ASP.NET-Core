using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi4.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {

        // GET http://localhost:5040/api/Region

        //[Authorize]
        //[Authorize(Roles = "admin")]
        //[Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        [Authorize( Policy = "CityPolicy" )]
        [HttpGet]
        public IActionResult GetInfo()
        {

            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;


            // Ort aus den Claims holen
            var userOrtClaim = User.Claims.FirstOrDefault(x => x.Type == "city");
            var userOrt = userOrtClaim?.Value ?? "Unbekannt";

            //return Ok(new { UserName = userName });
            //return Ok($" Community => UserName: {userName}; UserId: {userId}");
            return Ok($"Region => UserName: {userName}; UserId: {userId}; Ort: {userOrt}");

        }

    }
}
