using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdultController : ControllerBase
    {



        // http://localhost:5020/api/Manager

        //[Authorize]
        //[Authorize(Roles = "admin")]
        [Authorize(Policy = "AgePolicy")]
        //[Authorize(Roles = "admin", Policy = "CityPolicy")]
        [HttpGet]
        public IActionResult GetInfo()
        {

            // Benutzernamen auslesen
            var userName = User.Identity?.Name;

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value ?? string.Empty;


            // Geburtsdatum aus den Claims holen
            var birthDateClaim = User.Claims.FirstOrDefault(x => x.Type == "birth-date");
            DateTime birthDate;
            int userAlter = -1;

            if (birthDateClaim != null && DateTime.TryParse(birthDateClaim.Value, out birthDate))
            {
                // Alter berechnen
                var today = DateTime.Today;
                userAlter = today.Year - birthDate.Year;

                // Prüfen, ob der Geburtstag in diesem Jahr noch nicht vergangen ist
                // Falls ja, wird das Alter um eins reduziert
                if (birthDate > today.AddYears(-userAlter))
                {
                    userAlter--; // Alter korrigieren, da Geburtstag noch nicht war
                }
            }

            //return Ok(new { UserName = userName });
            //return Ok($" Community => UserName: {userName}; UserId: {userId}");
            return Ok($"Adult => UserName: {userName}; UserId: {userId}; Alter: {userAlter}");

        }


    }
}
