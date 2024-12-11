using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApi6.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {


        // Nur Benutzer können darauf zugreifen (UserToken)
        [Authorize(AuthenticationSchemes = "UserToken")]
        [HttpGet("User")]
        public IActionResult GetUserInfo()
        {
            // Benutzeroperationen

            return Ok($"Community => UserInfo ");
        }


        // Nur Clients können darauf zugreifen (ClientToken)
        [Authorize(AuthenticationSchemes = "ClientToken")]
        [HttpGet("Client")]
        public IActionResult GetClientInfo()
        {
            // Clientoperationen

            return Ok($"Community => ClientInfo ");
        }


    }
}
